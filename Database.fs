module Database
(*
open System
open FSharp.Data.Sql

let [<Literal>] dbVendor    = Common.DatabaseProviderTypes.MSSQLSERVER
let [<Literal>] connString  = "Server=localhost;Database=Genesys;Integrated Security=sspi"
//let [<Literal>] connString  = @"""Server=localhost;Database=Genesys;User Id=mike\miked;Password='' """
let [<Literal>] useOptTypes = FSharp.Data.Sql.Common.NullableColumnType.OPTION
let [<Literal>] indivAmount = 1000

type sql =
    SqlDataProvider<
        dbVendor,
        connString,
        IndividualsAmount = indivAmount,
        UseOptionTypes = useOptTypes>

let ctx = sql.GetDataContext()

let domainNameSeq =
    query {
        for domain in ctx.Dbo.Domain do
        where (domain.DomainId = 1)
        select (domain.DomainName)
    }

let domainName = sprintf "%A" domainNameSeq

*)

(*
open Dapper.FSharp.MSSQL
open Step.Types

let [<Literal>] connString = "Server=localhost;Database=Genesys;Integrated Security=sspi"

let getConnection () =
    new Microsoft.Data.SqlClient.SqlConnection( connString )

let conn = getConnection ()

type Domain = {
    DomainId      : int64
    ContextId     : int64
    DomainName    : string
    DomainDoc     : string
    DomainDeleted : bool
}

let domainTable = table<Domain> |> inSchema "dbo"

let domains = select {
    for d in domainTable do
        where (d.DomainId = 1 )
} 
// |> conn.SelectAsync<Domain>

let domainsText = sprintf "domainsText = %A" domains
*)

open FSharp.Data

type Domain = {
    DomainId      : int64
    ContextId     : int64
    DomainName    : string
    DomainDoc     : string
    DomainDeleted : bool
}

let [<Literal>] connString = "Server=localhost;Database=Genesys;Integrated Security=sspi"

type DomainProvider = SqlCommandProvider<" SELECT * FROM Domain", connString>

let cmd: DomainProvider = new DomainProvider( connString )
type DomainT = DomainProvider
let domain = cmd.Execute() |> Seq.head

let dom: Domain = { 
                    DomainId      = domain.DomainId
                    ContextId     = domain.ContextId
                    DomainName    = domain.DomainName
                    DomainDoc     = domain.DomainDoc
                    DomainDeleted = domain.DomainDeleted
                  }


let domainString = sprintf "%A" domain.DomainDoc
