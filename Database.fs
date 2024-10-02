module Database

open FSharp.Data

exception DatabaseError of string

let [<Literal>] connString = "Server=localhost;Database=Genesys;Integrated Security=sspi"

type DomainPT = SqlCommandProvider< "SELECT * FROM Domain", connString>

let domainPT = new DomainPT( connString )
let domain   = domainPT.Execute() |> Seq.head
let domainString = sprintf "%A" domain
