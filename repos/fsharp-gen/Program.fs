open System.IO
open FSharp.Data


[<EntryPoint>]
let main argv =
    let jsonPath   =  @"..\..\..\json\Genesys.json"
    let jsonString = File.ReadAllText( jsonPath )

    let Genesys = JsonProvider<jsonString>

    printfn "jsonString = %s" jsonString
    exit 0
     