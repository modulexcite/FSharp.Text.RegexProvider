module FSharp.Text.RegexProvider.Tests.RegexProviderTests

open NUnit.Framework
open System.Text.RegularExpressions
open FSharp.Text.RegexProvider
open FsUnit

type PhoneRegex = Regex< @"(?<AreaCode>^\d{3})-(?<PhoneNumber>\d{3}-\d{4}$)">

[<Test>] 
let ``Can call typed IsMatch function``() =      
    PhoneRegex.IsMatch "425-123-2345"
    |> should equal true

[<Test>] 
let ``The default regex options are None``() =      
    PhoneRegex().Options |> should equal RegexOptions.None

[<Test>] 
let ``The regex options are passed``() =      
    PhoneRegex(RegexOptions.Compiled).Options |> should equal RegexOptions.Compiled

[<Test>] 
let ``Can call typed CompleteMatch function``() =      
    PhoneRegex().Match("425-123-2345").CompleteMatch.Value
    |> should equal "425-123-2345"

[<Test>] 
let ``Can return AreaCode in simple phone number``() =
    PhoneRegex().Match("425-123-2345").AreaCode.Value
    |> should equal "425"

[<Test>] 
let ``Can return PhoneNumber property in simple phone number``() =
    PhoneRegex().Match("425-123-2345").PhoneNumber.Value
    |> should equal "123-2345"

type MultiplePhoneRegex = Regex< @"\b(?<AreaCode>\d{3})-(?<PhoneNumber>\d{3}-\d{4})\b" >
[<Test>]
let ``Can return multiple matches``() =
    MultiplePhoneRegex().Matches("425-123-2345, 426-123-2346, 427-123-2347")
    |> Seq.map (fun x -> x.AreaCode.Value)
    |> List.ofSeq
    |> should equal ["425"; "426"; "427"]


[<Test>]
let ``Can return next matches``() =
    let m = MultiplePhoneRegex().Match("425-123-2345, 426-123-2346, 427-123-2347")
    m.AreaCode.Value
    |> should equal "425"

    let m' = m.NextMatch()
    m'.AreaCode.Value
    |> should equal "426"
