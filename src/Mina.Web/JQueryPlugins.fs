namespace Mina.Web

open WebSharper
open WebSharper.UI
open WebSharper.UI.Html
open WebSharper.UI.Client
open WebSharper.JavaScript
open WebSharper.JQuery

module JQueryPieProgressResources =
    open WebSharper.Core.Resources
    
    type CSS() =
        inherit BaseResource("asPieProgress.min.css")
  
    type JS() =
        inherit BaseResource("jquery-asPieProgress.min.js")

[<JavaScript>]
type JQueryPieProgressOptions = {
    [<Name "namespace">]
    ns: string
    [<Name "goal">]
    Goal: int
    [<Name "first">]
    First: int
    [<Name "min">]
    Min: int
    [<Name "max">]
    Max: int
    [<Name "speed">]
    Speed: int
    [<Name "easing">]
    Easing: string
    [<Name "numberCallback">]
    NumberCallback:Optional<int->string>
}
        
[<Require(typeof<JQueryPieProgressResources.CSS>);Require(typeof<JQuery.Resources.JQuery>);Require(typeof<JQueryPieProgressResources.JS>)>]
[<JavaScript>]
module JQueryPieProgress =
    [<Inline "jQuery($el).asPieProgress($options)">]
    let enable (el:Dom.Element) (options:JQueryPieProgressOptions) = X<unit>

    [<Inline "jQuery($el).asPieProgress('start')">]
    let start (el:Dom.Element) = X<unit>