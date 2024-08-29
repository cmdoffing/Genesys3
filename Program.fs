open WelcomeUsers

[<EntryPoint>]
let main args =
    let fileName = System.IO.Path.GetTempFileName() + ".html"

    args
    |> Seq.toList
    |> WelcomeUsers.render
    |> fun x -> System.IO.File.WriteAllText( fileName, x )
    |> ignore

    let p = new System.Diagnostics.Process()
    p.StartInfo.FileName <- fileName
    p.StartInfo.UseShellExecute <- true
    p.Start() |> ignore

    0