namespace Game

open System
open Microsoft.Xna.Framework;
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Content
open Microsoft.Xna.Framework.Input

// PrimitiveBatch is a class that handles efficient rendering automatically for its
// users, in a similar way to SpriteBatch. PrimitiveBatch can render lines, points,
// and triangles to the screen. In this sample, it is used to draw a spacewars
// retro scene.
type PrimitiveBatch (graphicsDevice:GraphicsDevice) = // : IDisposable
    // this constant controls how large the vertices buffer is. Larger buffers will
    // require flushing less often, which can increase performance. However, having
    // buffer that is unnecessarily large will waste memory.
    let DefaultBufferSize = 500

    // a block of vertices that calling AddVertex will fill. Flush will draw using
    // this array, and will determine how many primitives to draw from
    // positionInBuffer.
    let vertices : VertexPositionColor[] = Array.zeroCreate DefaultBufferSize

    // keeps track of how many vertices have been added. this value increases until
    // we run out of space in the buffer, at which time Flush is automatically
    // called.
    let mutable positionInBuffer = 0

    // this value is set by Begin, and is the type of primitives that we are
    // drawing.
    let mutable _primitiveType = Unchecked.defaultof<PrimitiveType>

    // how many verts does each of these primitives take up? points are 1,
    // lines are 2, and triangles are 3.
    let mutable numVertsPerPrimitive = 0

    // hasBegun is flipped to true once Begin is called, and is used to make
    // sure users don't call End before Begin is called.
    let mutable hasBegun = false;

    let mutable isDisposed = false;

    // the constructor creates a new PrimitiveBatch and sets up all of the internals
    // that PrimitiveBatch will need.
    do  if (graphicsDevice = null) then raise <| ArgumentNullException("graphicsDevice")
         
    let device = graphicsDevice

    // set up a new basic effect, and enable vertex colors.
    let basicEffect = new BasicEffect(graphicsDevice);
    do  basicEffect.VertexColorEnabled <- true;

    // projection uses CreateOrthographicOffCenter to create 2d projection
    // matrix with 0,0 in the upper left.
    do  basicEffect.Projection <- 
            Matrix.CreateOrthographicOffCenter
                (0.0f, float32 graphicsDevice.Viewport.Width,
                 float32 graphicsDevice.Viewport.Height, 0.0f,
                 0.0f, 1.0f);

    interface IDisposable with
        member this.Dispose() =
            this.Dispose(true)
            GC.SuppressFinalize(this)
      
    member this.Dispose(disposing:bool) =
        if (disposing && not isDisposed) then
            if (basicEffect <> null) then basicEffect.Dispose()
            isDisposed <- true

    // Begin is called to tell the PrimitiveBatch what kind of primitives will be
    // drawn, and to prepare the graphics card to render those primitives.
    member this.Begin(primitiveType:PrimitiveType) =
        if (hasBegun) 
        then
            raise <| InvalidOperationException("End must be called before Begin can be called again.")

        // these three types reuse vertices, so we can't flush properly without more
        // complex logic. Since that's a bit too complicated for this sample, we'll
        // simply disallow them.
        if (primitiveType = PrimitiveType.LineStrip ||
            primitiveType = PrimitiveType.TriangleStrip)
        then
            raise <| NotSupportedException("The specified primitiveType is not supported by PrimitiveBatch.")   

        _primitiveType <- primitiveType

        // how many verts will each of these primitives require?
        numVertsPerPrimitive <- PrimitiveBatch.NumVertsPerPrimitive(primitiveType)

        //tell our basic effect to begin.
        basicEffect.CurrentTechnique.Passes.[0].Apply()

        // flip the error checking boolean. It's now ok to call AddVertex, Flush,
        // and End.
        hasBegun <- true;

    // AddVertex is called to add another vertex to be rendered. To draw a point,
    // AddVertex must be called once. for lines, twice, and for triangles 3 times.
    // this function can only be called once begin has been called.
    // if there is not enough room in the vertices buffer, Flush is called
    // automatically.
    member this.AddVertex(vertex:Vector2, color:Color) =
        
        if (not hasBegun)
        then
            raise <| InvalidOperationException("Begin must be called before AddVertex can be called."); 

        // are we starting a new primitive? if so, and there will not be enough room
        // for a whole primitive, flush.
        let newPrimitive = ((positionInBuffer % numVertsPerPrimitive) = 0)

        if (newPrimitive && (positionInBuffer + numVertsPerPrimitive) >= vertices.Length)
        then
            this.Flush()

        // once we know there's enough room, set the vertex in the buffer,
        // and increase position.
        vertices.[positionInBuffer].Position <- Vector3(vertex, 0.0f)
        vertices.[positionInBuffer].Color <- color;

        positionInBuffer <- positionInBuffer + 1

    // End is called once all the primitives have been drawn using AddVertex.
    // it will call Flush to actually submit the draw call to the graphics card, and
    // then tell the basic effect to end.
    member this.End() =
        if (not hasBegun)
        then
            raise <| InvalidOperationException("Begin must be called before End can be called.")     

        // Draw whatever the user wanted us to draw
        this.Flush()

        hasBegun <- false   

    // Flush is called to issue the draw call to the graphics card. Once the draw
    // call is made, positionInBuffer is reset, so that AddVertex can start over
    // at the beginning. End will call this to draw the primitives that the user
    // requested, and AddVertex will call this if there is not enough room in the
    // buffer.
    member private this.Flush() =
        if (not hasBegun)
        then
            raise <| InvalidOperationException("Begin must be called before Flush can be called.");

        // no work to do
        if (positionInBuffer = 0)
        then ()
        else
            // how many primitives will we draw?
            let primitiveCount = positionInBuffer / numVertsPerPrimitive

            // submit the draw call to the graphics card
            device.DrawUserPrimitives<VertexPositionColor>(_primitiveType, vertices, 0,
                primitiveCount)

            // now that we've drawn, it's ok to reset positionInBuffer back to zero,
            // and write over any vertices that may have been set previously.
            positionInBuffer <- 0

    // NumVertsPerPrimitive is a boring helper function that tells how many vertices
    // it will take to draw each kind of primitive.
    static member private NumVertsPerPrimitive(primitive:PrimitiveType) =
        match primitive with
        | PrimitiveType.LineList -> 2
        | PrimitiveType.TriangleList -> 3
        | _ -> raise <| InvalidOperationException("primitive is not valid")
