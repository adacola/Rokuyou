namespace Adacola

type Rokuyou =
    | Taian = 0
    | Shakku = 1
    | Senkachi = 2
    | Tomobiki = 3
    | Senmake = 4
    | Butsumetsu = 5

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Rokuyou =
    open System
    open System.Globalization

    [<CompiledName("ToString")>]
    let toString = function
        | Rokuyou.Taian -> "大安"
        | Rokuyou.Shakku -> "赤口"
        | Rokuyou.Senkachi -> "先勝"
        | Rokuyou.Tomobiki -> "友引"
        | Rokuyou.Senmake -> "先負"
        | Rokuyou.Butsumetsu -> "仏滅"
        | _ -> invalidArg "" "Rokuyou型の値を指定してください"

    let private ja = JapaneseLunisolarCalendar()

    [<CompiledName("OfDate")>]
    let ofDate (date : DateTime) =
        // 参考 : http://blogs.jp.infragistics.com/blogs/dikehara/archive/2010/08/29/wpf-xammonthcalendar-tips.aspx
        let oldMonth =
            let newYearDate = ja.AddDays(date, 1 - ja.GetDayOfYear date)
            let leapMonth = ja.GetLeapMonth(ja.GetYear newYearDate, ja.GetEra newYearDate)
            let month = ja.GetMonth date
            if  0 < leapMonth && leapMonth <= month then month - 1 else month
        let oldDay = ja.GetDayOfMonth date
        (oldMonth + oldDay) % 6 |> enum<Rokuyou>
