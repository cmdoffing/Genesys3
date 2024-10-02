module Urls

let domainDefaultUrl = "/domain"
let domainIndexUrl   = "/domain/index"
let domainNewUrl     = "/domain/new"
let domainUpdateUrl  = "/domain/update"      // Used with POST, so no id parameter needed
let domainInsertUrl  = "/domain/insert"      // Used with POST, so no id parameter needed
let domainDeleteUrl  = "/domain/delete/%d"   // Fix change id group to Guid. Make PrintfFormat instead of string