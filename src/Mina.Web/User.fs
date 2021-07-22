namespace Mina.Web

open System.Collections.Generic

open WebSharper
open WebSharper.UI
open WebSharper.JavaScript

open WebSharper.JQuery
open Mina.Models
open Mina.NLU

[<JavaScript>]
module User =
    let name = "User"
    let debug m = ClientExtensions.debug name m
    
    /// Update the dialogue state
    let rec update d =        
        Dialogue.debugInterpreterStart d debug name

        let (Dialogue.Dialogue(cui, props, dialogueQuestions, output, utterances)) = d
        
        let echo = Dialogue.echo d
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

        (* Base dialogue patterns *)
        let (|Agenda|_|) = Dialogue.(|Agenda_|_|) d
        let (|PropSet|_|) = Dialogue.(|PropSet_|_|) d
        let (|PropNotSet|_|) = Dialogue.(|PropNotSet_|_|) d
        let (|User|_|) = Dialogue.(|User_|_|) d
        let (|User'|_|) = Dialogue.(|User'_|_|) d
        let (|Intent|_|) = Dialogue.(|Intent_|_|) d
        let (|Intent'|_|) = Dialogue.(|Intent'_|_|) d
        let (|Response|_|) = Dialogue.(|Response_|_|) d
        let (|Response'|_|) = Dialogue.(|Response'_|_|) d
       
        let user():User = prop "user"
        let triggerTests = Dialogue.trigger d debug Tests.update

        (* User functions *)
        let switchUserQuestion u = Question("switchUser", name, Verification ((fun _ -> trigger "verify" "yes"), (fun _ -> trigger "reject" "no")), None, fun _ -> say <| sprintf "Do you want me to switch to the user %s" u)

         
        let loginUser u = 
            do sayRandom waitRetrievePhrases "user name"
            async { 
                match! Server.getUser u with 
                | Some user ->
                    do! Server.updateUserLastLogin user.Name |> Async.Ignore
                    sayRandom helloUserPhrases user.Name
                    add "user" u                    
                    if Option.isSome user.LastLoggedIn then 
                        let! h = Server.humanize user.LastLoggedIn.Value
                        say <| sprintf "You last logged in %s." h
                    doc <| Doc.Concat [
                        Bs.btnPrimary "tests" (fun _ _ -> triggerTests "list_test_categories" "list_test_categories")
                        Html.text "     "
                        Bs.btnInfo "help" (fun _ _ -> trigger "help" "help")
                    ]
                | None _ -> 
                    say <| sprintf "I did not find a user with the name %s." u
                    Question("addUser", name, Verification ((fun _ -> trigger "verify" "yes"), (fun _ -> trigger "reject" "no")), None, fun _ -> add "addUser" u; say <| sprintf "Do you want me to add the user %s?" u) |> ask
            } |> Async.Start
        
        let addUser u = 
            async { 
                do sayRandom waitAddPhrases "user"
                match! Server.addUser u with 
                | Ok _ -> 
                        add "user" u
                        add "newuser" true
                        say <| sprintf "Hello %A, nice to meet you." props.["user"]
                        say "Click on one of the buttons below to get a list of writing prompts."
                | Error e -> 
                    error <| sprintf "Error adding user %s:%s." u e
                    say <| sprintf "Sorry I was not able to add the user %s to the system." u
            } |> Async.Start
          
        (* Interpreter logic begins here *)
        match Dialogue.frame utterances with
        
        (* User login *)
        | Intent' "greet" (_, Entity1OfAny "name" u)::[]
        | Intent' "greet" (_, Entity1OfAny "contact" u)::[]-> handle "loginUser" (fun _ -> loginUser u.Value)
        | Intent' "hello" (_, Entity1OfAny "contact" u)::[] -> handle "loginUser" (fun _ -> loginUser u.Value)
        
        (* User add *)
        | No(Response' "addUser" (_, _, PStr u))::[] -> endt "addUser" (fun _ -> say <| sprintf "Ok I did not add the user %s. But you must login for me to help you." u)
        | Yes(Response' "addUser" (_, _, PStr u))::[] -> endt "addUser" (fun _ -> addUser u)
        
        (* User switch *)
        | Intent "greet" (None, Entity1Of1 "name" u)::[] -> 
            async {
                match! Server.getUser u.Value with
                | Some user -> switchUserQuestion user.Name |> ask
                | None -> say <| sprintf "Sorry, the user %s does not exist." u.Value
            } |> Async.Start
        | Yes(Response "switchUser" (_, _, PStr user))::[] ->
            props.["user"] <- user
            if have "newuser" then remove "newuser"
            say <| sprintf "Ok I switched to user %A." user  
        | No(Response "switchUser" (_, _, PStr user))::[] -> 
            say <| sprintf "Ok I did not switch to user %s." user
                
        | _ -> didNotUnderstand()

        Dialogue.debugInterpreterEnd d debug name
