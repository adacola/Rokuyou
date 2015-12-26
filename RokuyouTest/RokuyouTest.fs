namespace Adacola.Rokuyou.Test

open FsCheck
open NUnit.Framework

[<TestFixture>]
module Rokuyou =
    open System
    open System.IO
    open Adacola

    [<Test>]
    let ``ofDateで取得した六曜とその文字列が妥当であること`` () =
        let expectedMap =
            let lines = File.ReadAllLines(@"RokuyouExpected.tsv", Text.Encoding.UTF8)
            lines |> Seq.map (fun line ->
                let splittedLine = line.Split [|'\t'|]
                let date = DateTime.ParseExact(splittedLine.[0], "yyyy/MM/dd", null)
                let rokuyou = splittedLine.[2] |> int |> enum<Rokuyou>
                date, (rokuyou, splittedLine.[1]))
            |> dict

        let dateTimeGen = gen {
            let year = 2016
            let! month = Gen.choose(1, 12)
            let maxDay = DateTime.DaysInMonth(year, month)
            let! day = Gen.choose(1, maxDay)
            return DateTime(year, month, day)
        }

        Prop.forAll (Arb.fromGen dateTimeGen) (fun dt ->
            let rokuyou = Rokuyou.ofDate dt
            let rokuyouStr = Rokuyou.toString rokuyou
            (rokuyou, rokuyouStr) = expectedMap.[dt])
        |> Check.QuickThrowOnFailure
