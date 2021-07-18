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

        (* Base dialogue patterns *)
        let (|Agenda|_|) = Dialogue.(|Agenda_|_|) d
        let (|PropSet|_|) = Dialogue.(|PropSet_|_|) d
        let (|PropNotSet|_|) = Dialogue.(|PropNotSet_|_|) d
        let (|User|_|) = Dialogue.(|User_|_|) d
        let (|User'|_|) = Dialogue.(|User'_|_|) d
        let (|Response|_|) = Dialogue.(|Response_|_|) d
        let (|Response'|_|) = Dialogue.(|Response'_|_|) d
       
        let user():User = prop "user"
        let triggerJournal = Dialogue.trigger d debug Journal.update
          
        let testCategories = ["Psychological"; "Cognitive"; "Neurological"]
        (* Interpreter logic begins here *)
        match Dialogue.frame utterances with
                
        | User(Intent "list_test_categories" _)::[] -> 
            say "Choose one of the test categories below."
            cui.ListMenuItems testCategories trigger
        | User(Intent "query" _)::[]
        | User(Intent "medication_journal" _)::[] -> Journal.update d

        | _ -> didNotUnderstand()

        Dialogue.debugInterpreterEnd d debug name
