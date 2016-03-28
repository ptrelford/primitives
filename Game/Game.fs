// Learn more about F# at http://fsharp.net

namespace Game

open System
open System.Collections.Generic
open System.Linq
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Audio
open Microsoft.Xna.Framework.Content
open Microsoft.Xna.Framework.GamerServices
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input
open Microsoft.Xna.Framework.Input.Touch
open Microsoft.Xna.Framework.Media

/// <summary>
/// This is the main type for your game
/// </summary>
type Game() as this = 
    inherit Microsoft.Xna.Framework.Game()
    let graphics = new GraphicsDeviceManager(this)
    let mutable primitiveBatch = Unchecked.defaultof<PrimitiveBatch>
    let mutable spriteBatch = null
    let mutable font : SpriteFont = null
    
    do this.Content.RootDirectory <- "Content"

    // Frame rate is 30 fps by default for Windows Phone.
    do this.TargetElapsedTime <- TimeSpan.FromTicks(333333L)

    do graphics.PreferredBackBufferWidth <- 480
    do graphics.PreferredBackBufferHeight <- 800

    /// <summary>
    /// Allows the game to perform any initialization it needs to before starting to run.
    /// This is where it can query for any required services and load any non-graphic
    /// related content.  Calling base.Initialize will enumerate through any components
    /// and initialize them as well.
    /// </summary>
    override this.Initialize() = 
        // TODO: Add your initialization logic here
        base.Initialize()

    /// <summary>
    /// LoadContent will be called once per game and is the place to load
    /// all of your content.
    /// </summary>
    override this.LoadContent() =
        primitiveBatch <- new PrimitiveBatch(this.GraphicsDevice)

        // Create a new SpriteBatch, which can be used to draw textures.
        spriteBatch <- new SpriteBatch(this.GraphicsDevice)

        // Load the sprite font from the GameHostContent project.
        font <- this.Content.Load("font")

        // TODO: use this.Content to load your game content here

        // Now that spriteBatch and font are initialized, we can instantiate HelloWorldComponent
        let helloWorldComponent = new RotatingTextComponent(this, spriteBatch, font)
        this.Components.Add(helloWorldComponent)

        let stars = new StarsComponent(this, graphics)
        this.Components.Add(stars)

    /// <summary>
    /// UnloadContent will be called once per game and is the place to unload
    /// all content.
    /// </summary>
    override this.UnloadContent() =
        // TODO: Unload any non ContentManager content here
        ()

    /// <summary>
    /// Allows the game to run logic such as updating the world,
    /// checking for collisions, gathering input, and playing audio.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    override this.Update(gameTime : GameTime) =
        // Allows the game to exit
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back = ButtonState.Pressed) then
            this.Exit()

        // TODO: Add your update logic here

        base.Update(gameTime)

    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    override this.Draw(gameTime: GameTime) =
        this.GraphicsDevice.Clear(Color.Black)

        // TODO: Add your drawing code here

        let screenWidth = float32 graphics.GraphicsDevice.Viewport.Width;
        let screenHeight = float32 graphics.GraphicsDevice.Viewport.Height

        // draw the sun in the center
        Sun.draw(primitiveBatch,Vector2(screenWidth / 2.0f, screenHeight / 2.0f))

        // draw the left hand ship
        Ship.draw(primitiveBatch,Vector2(100.0f, screenHeight / 2.0f))

        // and the right hand ship
        Ship.draw(primitiveBatch,Vector2(screenWidth - 100.0f, screenHeight / 2.0f))

        base.Draw(gameTime)


