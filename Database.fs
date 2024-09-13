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