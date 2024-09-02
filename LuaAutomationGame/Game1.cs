using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myra.Graphics2D.UI;
using Myra;

namespace LuaAutomationGame;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Desktop _desktop;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        MyraEnvironment.Game = this;
        
        var grid = new Grid
        {
          RowSpacing = 8,
          ColumnSpacing = 8
        };
        
        grid.ColumnsProportions.Add(new Proportion(ProportionType.Auto));
        grid.ColumnsProportions.Add(new Proportion(ProportionType.Auto));
        grid.RowsProportions.Add(new Proportion(ProportionType.Auto));
        grid.RowsProportions.Add(new Proportion(ProportionType.Auto));
        
        var helloWorld = new Label
        {
          Id = "label",
          Text = "Hello, World!"
        };
        grid.Widgets.Add(helloWorld);
        
        // ComboBox
        var combo = new ComboBox();
        Grid.SetColumn(combo, 1);
        Grid.SetRow(combo, 0);
        
        combo.Items.Add(new ListItem("Red", Color.Red));
        combo.Items.Add(new ListItem("Green", Color.Green));
        combo.Items.Add(new ListItem("Blue", Color.Blue));
        grid.Widgets.Add(combo);
        
        // Button
        var button = new Button
        {
          Content = new Label
          {
            Text = "Show"
          }
        };
        Grid.SetColumn(button, 0);
        Grid.SetRow(button, 1);
        
        button.Click += (s, a) =>
        {
          var messageBox = Dialog.CreateMessageBox("Message", "Some message!");
          messageBox.ShowModal(_desktop);
        };
        
        grid.Widgets.Add(button);
        
        // Spin button
        var spinButton = new SpinButton
        {
          Width = 100,
          Nullable = true
        };
        Grid.SetColumn(spinButton, 1);
        Grid.SetRow(spinButton, 1);
        
        grid.Widgets.Add(spinButton);
        
        // Add it to the desktop
        _desktop = new Desktop();
        _desktop.Root = grid;
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _desktop.Render();

        base.Draw(gameTime);
    }
}