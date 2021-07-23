namespace Mina.Web

open System.Collections.Generic

open WebSharper
open WebSharper.UI
open WebSharper.JavaScript
open WebSharper.UI.Html
open WebSharper.JQuery
open Mina.Models
open Mina.NLU

[<JavaScript>]
module Tests =
    let name = "Tests"
    let debug m = ClientExtensions.debug name m
    
    /// Update the dialogue state
    let rec update d =        
        Dialogue.debugInterpreterStart d debug name

        let (Dialogue.Dialogue(cui, props, dialogueQuestions, output, utterances)) = d
        
        let echo = Dialogue.echo d
        let print = echo
        let say' = Dialogue.say' d
        let say = Dialogue.say d
        let doc = cui.EchoDoc
        let sayRandom = Dialogue.sayRandom d
        let sayRandom' = Dialogue.sayRandom' d

        (* Manage the dialogue state elements*)
        let have = Dialogue.have d 
        let prop k  = Dialogue.prop d k
        let add k v = Dialogue.add d debug k v
        let remove = Dialogue.remove d debug

        let pushu = Dialogue.pushu d debug
        let pushq = Dialogue.pushq d debug
        let popu() = Dialogue.popu d debug
        let popq() = Dialogue.popq d debug
        
        let dispatch = Dialogue.dispatch d debug
        let handle = Dialogue.handle d debug
        let trigger = Dialogue.trigger d debug update
        let trigger' = Dialogue.trigger' d debug update
        let cancel = Dialogue.cancel d debug
        let endt = Dialogue.endt d debug
        let endt' = Dialogue.endt' d debug
        let didNotUnderstand() = Dialogue.didNotUnderstand d debug name

        let ask = Questions.ask d debug
        let menu = Questions.menu d "Tests"

        (* Base dialogue patterns *)
        let (|Agenda|_|) = Dialogue.(|Agenda_|_|) d
        let (|PropSet|_|) = Dialogue.(|PropSet_|_|) d
        let (|PropNotSet|_|) = Dialogue.(|PropNotSet_|_|) d
        let (|User|_|) = Dialogue.(|User_|_|) d
        let (|User'|_|) = Dialogue.(|User'_|_|) d
        let (|Intent|_|) = Dialogue.(|Intent_|_|) d
        let (|Response|_|) = Dialogue.(|Response_|_|) d
        let (|Response'|_|) = Dialogue.(|Response'_|_|) d
       
        let user():User = prop "user"
        let triggerJournal = Dialogue.trigger d debug Journal.update
          
        let testCategories = ["Physical Health Tests"; "Mental Health Tests"; "Cognitive Tests"; "Psychological Tests"]
        
        let physicalHealthTests = ["Bladder Control Scale"; "Bowel Control Scale"; "Modified Fatigue Impact Scale"; "MOS Pain Effects Scale"; "Sexual Satisfaction Scale"]
        let mentalHealthTests = ["Mental Health Inventory"; "Modified Social Support Survey"]
        let cognitiveTests = ["Perceived Deficits Questionnaire"; "Paced Auditory Serial Test"; "Single Digit Modality Test"]
        let psychologicalTests = ["Beck's Depression Inventory"]
        
        let pasatTestInstructions = "You are going to hear a seriesof single digit numbers that will be presented at the rate of one every 3 seconds. 
                                     Listen for the first two numbers, add them up, and tell me your answer. 
                                     When you hear the next number, add it to the one you heard right before it. 
                                     Continue to add the next number to each preceding one. Remember you are not being asked to give me a running total, but rather the sum of the last two numbers that you heard."

        let sdmtTestInstructions = "SDMT"
        let sdmtCharacters = ["\u2540"; "\u2560"; "\u2599"; "\u25AE"; "\u25B3"; "\u25C9"; "\u25E0"; "\u25F0"; "\u2593"]
        
        (* Interpreter logic begins here *)

        match Dialogue.frame utterances with        
        
        | Intent "list_test_categories" _::[] -> 
            handle "list_test_categories" (fun _ -> 
                ask <| menu "menuTestCategories" testCategories "Choose one of the test categories from the list." trigger
            )

        | Response "menuTestCategories" (Intent "cancel" _,_,_)::[] -> endt "menuTestCategories" (fun _ ->
            doc <| Doc.Concat [
                Bs.btnPrimary "tests" (fun _ _ -> trigger "list_test_categories" "list_test_categories")
                Html.text "     "
                Bs.btnInfo "help" (fun _ _ -> trigger "help" "help")
            ]
          )

        | Response "menuTestCategories" (Number n,_,_)::[] -> 
            endt "menuTestCategories" (fun _ -> 
                match n with
                | 1 -> ask <| menu "menuPhysicalHealthTests" physicalHealthTests "Choose a physical health test from the list." trigger
                | 2 -> ask <| menu "menuMentalHealthTests" mentalHealthTests "Choose a mental health test from the list." trigger
                | 3 -> ask <| menu "menuCognitiveTests" cognitiveTests "Choose a cognitive test from the list." trigger
                | 4 -> ask <| menu "menuPsychologicalTests" psychologicalTests "Choose a psychological test from the list." trigger
                | _ -> say "Choose one of the test categories to see a list of tests available."
            )
        
        | Response "menuPhysicalHealthTests" (Intent "cancel" _,_,_)::[] 
        | Response "menuMentalHealthTests" (Intent "cancel" _,_,_)::[]
        | Response "menuCognitiveTests" (Intent "cancel" _,_,_)::[]
        | Response "menuPsychologicalTests" (Intent "cancel" _,_,_)::[] -> 
            endt' (fun _ ->
                ask <| menu "menuTestCategories" testCategories "Choose one of the test categories from the list." trigger
            )

        | Response "menuCognitiveTests" (Number n,_,_)::[] -> 
            endt "menuCognitiveTests" (fun _ -> 
                match n with
                | 2 -> 
                    doc <| Html.text "    "
                    say "You last took this test Monday. You're scheduled to take this test agan this week."
                    echo "The Paced Auditory Serial Addition Test (PASAT) is a measure of cognitive function that
                          specifically assesses auditory information processing speed and flexibility, as well as calculation ability."
                    doc <| Doc.Concat [
                        Html.text "     "
                        Bs.btnPrimary "start" (fun _ _ -> trigger "start_test_pasat" "start_test_pasat")
                        Html.text "     "
                        Bs.btnInfo "about" (fun _ _ -> trigger "about_test_pasat" "about_test_pasat")
                        Html.text "     "
                        Bs.btnSecondary "my history" (fun _ _ -> trigger "history_test_pasat" "history_test_pasat")
                        Html.text "     "
                        Bs.btnDark "cancel" (fun _ _ -> trigger "cancel" "cancel")
                    ]
                | 3 -> 
                    doc <| Html.text "    "
                    say "You last took this test Monday. You're scheduled to take this test agan this week."
                    echo "The Single Digit Modalities Test (SDMT) is a measure of cognitive function that
                          specifically assesses information processing speed and episodic memory."
                    doc <| Html.text "     "
                    doc <| Doc.Concat [
                        Bs.btnPrimary "start" (fun _ _ -> trigger "start_test_sdmt" "start_test_sdmt")
                        Html.text "     "
                        Bs.btnInfo "about" (fun _ _ -> trigger "about_test_sdmt" "about_test_sdmt")
                        Html.text "     "
                        Bs.btnSecondary "my history" (fun _ _ -> trigger "history_test_sdmt" "history_test_sdmt")
                        Html.text "     "
                        Bs.btnDark "cancel" (fun _ _ -> trigger "cancel" "cancel")
                    ]
                | _ -> say "Choose a cognitive test from the list."
            )
        | Intent "start_test_pasat" _::[] -> 
            handle "start_test_pasat" (fun _ ->
                say pasatTestInstructions
                echo "Listen to the instructions and click the Yes button when ready."
                ask <| Question("verify_start_test_pasat", name, Verification ((fun _ -> trigger "verify" "yes"), (fun _ -> trigger "reject" "no")), None, fun _ -> 
                    say "Are you ready to begin?";echo "Are you ready to begin?") 
            )
        
        | Yes(Response "verify_start_test_pasat"(_,_,_))::[] -> 
            JQuery("#testprofile").RemoveClass("invisible") |> ignore 
            JQuery("#testprofile-name").Text("PASAT") |>ignore
            JQueryPieProgress.enable (JS.Document.GetElementById("#testprofile-timer")) ({ns= "pie_progress"; Goal=0; First=120;Min=0; Max=120; Speed=1200;Easing="linear";NumberCallback = Defined (fun n -> 
            let minutes = int <| System.Math.Floor(float (JS.this?now) / 60.);
            let seconds = int <| JS.this?now % 60.;
            let min = (string) minutes
            let sec = if (seconds > 10) then (string) seconds else "0" + (string) seconds
            min + ": " + sec
            )})

            JQueryPieProgress.start (JS.Document.GetElementById("#testprofile-timer"))
            
        | Intent "start_test_sdmt" _::[] -> 
            handle "start_test_sdmt" (fun _ ->
                say sdmtTestInstructions
                echo "Listen to the instructions and click the Yes button when ready."
                ask <| Question("verify_start_test_sdmt", name, Verification ((fun _ -> trigger "verify" "yes"), (fun _ -> trigger "reject" "no")), None, fun _ -> 
                    say "Are you ready to begin?") 
            )
        
        | Yes(Response "verify_start_test_sdmt"(_,_,_))::[] -> 
            if JQuery("#testprofile").HasClass("invisible") then
                JQuery("#testprofile").RemoveClass("invisible").AddClass("visible") |> ignore 
            JQuery("#testprofile-name").Text("SDMT") |>ignore
            doc <| table [cls "table table-bordered"] [
                thead[] [tr [] (sdmtCharacters |> List.map(fun c -> th [Attr.Create "scope" "col"; attr.style "font-size:300%;text-align:center"] [text c]))]
                tbody[] [tr [] (sdmtCharacters |> List.mapi(fun i _ -> td [attr.style "font-size:300%;text-align:center"] [text <| string i]))]
            ]
            (*
            JQueryPieProgress.enable (JS.Document.GetElementById("#testprofile-timer")) ({ns= "pie_progress"; Goal=0; First=120;Min=0; Max=120; Speed=1200;Easing="linear";NumberCallback = Defined (fun n -> 
            let minutes = int <| System.Math.Floor(float (JS.this?now) / 60.);
            let seconds = int <| JS.this?now % 60.;
            let min = (string) minutes
            let sec = if (seconds > 10) then (string) seconds else "0" + (string) seconds
            min + ": " + sec
            )})
            *)
            JQueryPieProgress.start (JS.Document.GetElementById("#testprofile-timer"))

        | Intent "query" _::[]
        | Intent "medication_journal" _::[] -> Journal.update d

        | _ -> didNotUnderstand()

        Dialogue.debugInterpreterEnd d debug name
