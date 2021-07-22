namespace Mina.Web

open System.Collections.Generic

open WebSharper
open WebSharper.UI
open WebSharper.JavaScript

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
        | Response "menuPsychologicalTests" (Intent "cancel" _,_,_)::[]-> 
            endt "menuCognitiveTests" (fun _ ->
                ask <| menu "menuTestCategories" testCategories "Choose one of the test categories from the list." trigger
            )

        | Response "menuCognitiveTests" (Number n,_,_)::[] -> 
            endt "menuCognitiveTests" (fun _ -> 
                match n with
                | a when n > 0 && n <= 4  -> say cognitiveTests.[n - 1]
                | _ -> say "Choose a cognitive test from the list."
            )
        | Intent "query" _::[]
        | Intent "medication_journal" _::[] -> Journal.update d

        | _ -> didNotUnderstand()

        Dialogue.debugInterpreterEnd d debug name
