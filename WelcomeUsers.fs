module WelcomeUsers

open Giraffe.ViewEngine

let bodyTemplate (nameList: string list): XmlNode =
    body []
         [ h1 [] [ Text "Welcome:" ]
           ol [] (nameList |> List.map (fun x -> li [] [ Text x ])) ]

let navTemplate =
    nav [] [ a [ _href "./About" ] [ Text "About" ] ]

let documentTemplate (nav: XmlNode) (body: XmlNode) =
    html [] [ nav; body ]

let render welcomeUsers =
    bodyTemplate welcomeUsers
    |> (documentTemplate navTemplate)
    |> RenderView.AsString.htmlDocument