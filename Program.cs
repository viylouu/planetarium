using ImGuiNET;
using SimulationFramework;
using SimulationFramework.Desktop;
using SimulationFramework.Drawing;
using SimulationFramework.Input;
using System.Numerics;

Simulation sim = Simulation.Create(init, rend);
sim.Run(new DesktopPlatform());

partial class Program {
    static float cz = 1024;
    static Vector2 cp = new Vector2(Window.Width / 2, Window.Height / 2);

    static double G = (double)BigInteger.GreatestCommonDivisor(0, 1);

    static star sun = new star() {
        pos = Vector2.Zero,
        size = ctd(2715396),
        col = Color.Orange,
        name = "sun",
        mass = 198910000000000000000000000000.0
    };

    static planet earth = new planet {
        size = ctd(24901),
        orbitobj = sun,
        orbitdist = 92103583.1991,
        col = Color.Green,
        pos = Vector2.Zero,
        name = "earth",
        mass = 5972190000000000000000000.0
    };

    static planet jupiter = new planet {
        size = ctd(278985),
        orbitobj = sun,
        orbitdist = 595610356.766,
        col = Color.Tan,
        pos = Vector2.Zero,
        name = "jupiter",
        mass = 1898130000000000000000000000.0
    };

    static planet saturn = new planet { 
        size = ctd(235298),
        orbitobj = sun,
        orbitdist = 869919669.1323,
        col = Color.Tan,
        pos = Vector2.Zero,
        name = "saturn",
        mass = 568300000000000000000000000.0
    };

    static planet venus = new planet { 
        size = ctd(23627.64),
        orbitobj = sun,
        orbitdist = 67237911.8448,
        col = Color.Orange,
        pos = Vector2.Zero,
        name = "venus",
        mass = 4867000000000000000000000.0
    };

    static planet mars = new planet { 
        size = ctd(13261.925),
        orbitobj = sun,
        orbitdist = 141672631.83,
        col = Color.OrangeRed,
        pos = Vector2.Zero,
        name = "mars",
        mass = 639000000000000000000000.0
    };

    static planet uranus = new planet { 
        size = ctd(99739),
        orbitobj = sun,
        orbitdist = 1868084138.63,
        col = Color.LightBlue,
        pos = Vector2.Zero,
        name = "uranus",
        mass = 86810000000000000000000000.0
    };

    static planet mercury = new planet { 
        size = ctd(9525),
        orbitobj = sun,
        orbitdist = 36039529.15,
        col = Color.LightGray,
        pos = Vector2.Zero,
        name = "mercury",
        mass = 328500000000000000000000.0
    };

    static planet neptune = new planet { 
        size = ctd(96129),
        orbitobj = sun,
        orbitdist = 2779000000,
        col = Color.Cyan,
        pos = Vector2.Zero,
        name = "neptune",
        mass = 102400000000000000000000000.0
    };

    static planet[] sunorbits = new planet[] { earth, jupiter, saturn, venus, mars, uranus, mercury, neptune };

    static void init() { }

    static void rend(ICanvas c) {
        c.Clear(Color.Black);

        if (Mouse.IsButtonDown(MouseButton.Middle))
            cp -= Mouse.DeltaPosition * cz;

        cz -= Mouse.ScrollWheelDelta * (cz / 32f);
        cz = Math.Clamp(cz, 1, 999999999999999);

        c.Fill(sun.col);
        c.DrawCircle(wptsp(sun.pos).X, wptsp(sun.pos).Y, (float)wstss(sun.size / 2f), Alignment.Center);
        c.Fill(Color.White);
        c.DrawText(sun.name, wptsp(sun.pos), Alignment.Center);

        foreach (planet p in sunorbits) {
            c.Fill(p.col);
            c.DrawCircle(wptsp(p.pos).X, wptsp(p.pos).Y, (float)wstss(p.size / 2f), Alignment.Center);
            c.Fill(Color.White);
            c.DrawText(p.name, wptsp(p.pos), Alignment.Center);

            p.pos = p.orbitobj.pos + new Vector2(MathF.Sin(dtr((float)p.orbitprog)) * (float)p.orbitdist, MathF.Cos(dtr((float)p.orbitprog)) * (float)p.orbitdist);

            p.orbitprog += Time.DeltaTime * (Math.Sqrt(G * (p.orbitobj.mass + p.mass) / (p.orbitdist * 1609.34)) / 1609.34 / 360 / 96);
        }

        ImGui.Begin("status");

        ImGui.Text($"cpos:({cp.X},{cp.Y})");
        ImGui.Text($"czoom:{cz}");

        ImGui.End();

        if (Keyboard.IsKeyDown(Key.E))
            cp = earth.pos;
        if (Keyboard.IsKeyDown(Key.J))
            cp = jupiter.pos;
        if (Keyboard.IsKeyDown(Key.S))
            cp = saturn.pos;
        if (Keyboard.IsKeyDown(Key.V))
            cp = venus.pos;
        if (Keyboard.IsKeyDown(Key.M))
            cp = mars.pos;
        if (Keyboard.IsKeyDown(Key.U))
            cp = uranus.pos;
        if (Keyboard.IsKeyDown(Key.R))
            cp = mercury.pos;
        if (Keyboard.IsKeyDown(Key.N))
            cp = neptune.pos;
        if (Keyboard.IsKeyDown(Key.C))
            cp = Vector2.Zero;
    }

    class star : obj { 
        public double heat { get; set; }
    }

    class planet : obj { 
        public obj orbitobj { get; set; }
        public double orbitdist { get; set; }
        public double orbitprog { get; set; }
    }

    class obj { 
        public double mass { get; set; }
        public Vector2 pos { get; set; }
        public double size { get; set; }
        public Color col { get; set; }
        public string name { get; set; }
    }

    static Vector2 wptsp(Vector2 w) => (w - cp) / cz + new Vector2(Window.Width / 2, Window.Height / 2);
    static double wstss(double s) => s / cz;

    static double ctd(double circ) => circ / MathF.PI;

    static float dtr(float d) => d * (MathF.PI/180f);
}