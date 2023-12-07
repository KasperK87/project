namespace ResidentSurvivor {
    public static class HelperFunctionsEntities {
      public static void createAnimation(GameObject obj, int frame1){
        createAnimation(obj, frame1, frame1+75);
      }
      public static void createAnimation(GameObject obj, int frame1, int frame2){
        var frames = obj.frames;
        var frame1Obj = new SadConsole.ColoredString.ColoredGlyphEffect();
        frame1Obj.Glyph = frame1;
        frame1Obj.Foreground = SadRogue.Primitives.Color.White;
        frame1Obj.Background = SadRogue.Primitives.Color.Transparent;
        frames[0] = frame1Obj;
        frames[1] = frame1Obj;
        var frame2Obj = new SadConsole.ColoredString.ColoredGlyphEffect();
        frame2Obj.Glyph = frame2;
        frame2Obj.Foreground = SadRogue.Primitives.Color.White;
        frame2Obj.Background = SadRogue.Primitives.Color.Transparent;
        frames[2] = frame2Obj;
        frames[3] = frame2Obj;

        var anim = new SadConsole.Entities.AnimatedAppearanceComponent();
        anim.Frames = frames;

        anim.AnimationTime = TimeSpan.FromSeconds(2);
        anim.IsRepeatable = true;

        obj.SadComponents.Add(anim);

        if (obj.GetSadComponent<SadConsole.Entities.AnimatedAppearanceComponent>() != null){  
            obj.GetSadComponent<SadConsole.Entities.AnimatedAppearanceComponent>().Start();
        }
      }
    }
}