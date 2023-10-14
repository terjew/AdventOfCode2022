open System
open System.IO

type Vector = 
    { 
        X: int; 
        Y: int;
    }

module Vector =
    let U = {X =  0; Y =  1}
    let D = {X =  0; Y = -1}
    let L = {X = -1; Y = 0}
    let R = {X =  1; Y = 0}

    let abs (vec:Vector) : Vector =
        {
            X = Math.Abs vec.X; 
            Y = Math.Abs vec.Y
        }

    let clamp (vec:Vector) : Vector = 
        {
            X = Math.Clamp(vec.X, -1, 1);
            Y = Math.Clamp(vec.Y, -1, 1)
        }

    let max (vec:Vector) : int =
        let v1 = abs vec
        Math.Max (v1.X, v1.Y)

type Position = 
    { 
        X: int; 
        Y: int;
    }

    static member (-) ((posa : Position), (posb : Position)) : Vector =
        {X = posa.X - posb.X; Y = posa.Y - posb.Y}

    static member (+) ((pos : Position), (vec: Vector)) : Position =
        {X = pos.X + vec.X; Y = pos.Y + vec.Y}

module Position = 
    let move (offset : Vector) (position : Position) =
        position + offset

type Rope(head : Position, tail : Position, history : Set<Position>) = 
    member this.Head with get() = head
    member this.Tail with get() = tail
    member this.History with get() = history.Add tail

module Rope =
    let CalculateTail (head:Position) (tail:Position) : Position =
        let vec = head - tail
        let max = Vector.max vec
        match max with
            | 0 -> tail
            | 1 -> tail
            | _ -> tail + Vector.clamp vec

    let Move (moveFunc:Position->Position)  (rope : Rope) : Rope =
        let h1 = moveFunc(rope.Head)
        let t1 = CalculateTail h1 rope.Tail
        Rope(h1, t1, rope.History)

let readLines filePath = 
    File.ReadLines(filePath)

let notEmpty (s:string) = 
    String.IsNullOrEmpty(s) = false

let getMove (operation:String) : (Rope->Rope) =
    match operation with
        | "U" -> Rope.Move (Position.move Vector.U)
        | "D" -> Rope.Move (Position.move Vector.D)
        | "L" -> Rope.Move (Position.move Vector.L)
        | "R" -> Rope.Move (Position.move Vector.R)
        | _ -> failwith "wtf"

let getMoves (str:String) : seq<(Rope->Rope)> =
    let segments = str.Split " "
    let operation = segments[0]
    let count = int segments[1]
    let move = getMove operation
    Seq.replicate count move 

let solve path =
    let p0 = {X = 0; Y = 0}
    let initialRope = Rope(p0, p0, Set<Position>([]))
    readLines path
    |> Seq.filter notEmpty
    |> Seq.collect getMoves
    |> Seq.fold (fun rope moveFunc -> moveFunc rope) initialRope

let rope = solve "input.txt"

printfn "Number of positions visited: %A" rope.History.Count
