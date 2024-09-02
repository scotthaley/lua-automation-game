using DefaultEcs;
using DefaultEcs.System;
using LuaAutomationGame.Components.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LuaAutomationGame.Systems.Renderers;

public class SpriteRenderSystem(World world, ContentManager content)
    : AEntitySetSystem<SpriteBatch>(world.GetEntities().With<TransformComponent>().With<SpriteComponent>().AsSet())
{
    protected override void Update(SpriteBatch state, in Entity entity)
    {
        RenderSprite(state, entity, content);
    }
    
    public static void RenderSprite(SpriteBatch state, in Entity entity, ContentManager contentManager)
    {
        var transformPos = entity.Get<TransformComponent>().Position;
        
        RenderSprite(state, transformPos, entity, contentManager);
    }
    
    public static void RenderSprite(SpriteBatch state, Vector2 transformPos, in Entity entity, ContentManager contentManager)
    {
        var sprite = entity.Get<SpriteComponent>();
        if (sprite.TextureName == null) return;
        
        var texture = contentManager.Load<Texture2D>(sprite.TextureName);

        var width = texture.Width;
        var height = texture.Height;
        var sourceRectangle = new Rectangle(0, 0, width, height);
        
        var origin = Vector2.Zero;
        
        state.Draw(texture, transformPos, sourceRectangle, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
    }
}