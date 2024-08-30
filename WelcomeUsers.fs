module WelcomeUsers

open Giraffe.ViewEngine

// let bodyTemplate (nameList: string list): XmlNode =

let bodyTemplate =
    body []
         [ h1 [] [ Text "Welcome:" ]
           //ol [] (nameList |> List.map (fun x -> li [] [ Text x ]))
         ]

let navTemplate =
    nav [] [ a [ _href "/About" ] [ Text "About" ] ]

let documentTemplate =
    html [] [ navTemplate
              bodyTemplate
            ]

let aboutTemplate =
    p  [] [Text "About page"]

// let render welcomeUsers =
//    bodyTemplate welcomeUsers
//    |> (documentTemplate navTemplate)
//    |> RenderView.AsString.htmlDocument