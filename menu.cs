using System;
using Eto.Forms;
using Eto.Drawing;

public class MainForm : Form
{
    private Button[] gameTiles;
    private Button[] bottomMenu;
    private int selectedIndex = 0;
    private int bottomMenuIndex = 0;
    private bool isBottomMenuSelected = false;

    public MainForm()
    {
        Title = "Interfaz Inspirada en Nintendo Switch";
        ClientSize = new Size(1280, 720);
        WindowState = WindowState.Maximized;

        // Cerrar con tecla Escape
        KeyDown += (sender, e) =>
        {
            if (e.Key == Keys.Escape)
            {
                Application.Instance.Quit();
            }
        };

        gameTiles = new Button[]
        {
            CreateGameTile("[empty]"),
            CreateGameTile("[empty]"),
            CreateGameTile("[empty]"),
            CreateGameTile("[empty]"),
            CreateGameTile("[empty]"),
            CreateGameTile("[empty]"),
            CreateGameTile("[empty]"),
            CreateGameTile("[empty]")
        };

        bottomMenu = new Button[]
        {
            CreateBottomMenuButton("Noticias"),
            CreateBottomMenuButton("Nintendo eShop"),
            CreateBottomMenuButton("Álbum"),
            CreateBottomMenuButton("Controladores"),
            CreateBottomMenuButton("Configuración del Sistema"),
            CreateBottomMenuButton("Modo de Suspensión")
        };

        var timeLabel = new Label { Font = new Font(FontFamilies.Sans, 14), TextColor = Colors.Black };

        var mainLayout = new TableLayout
        {
            Padding = new Padding(20),
            Spacing = new Size(20, 20),
            Rows =
            {
                new TableRow(CreateTopBar(timeLabel)),
                new TableRow(CreateGameArea(0, 4)),
                new TableRow(CreateGameArea(4, 8)),
                new TableRow(CreateBottomBar())
            }
        };

        Content = mainLayout;

        // Actualizar la hora cada segundo
        var timer = new UITimer { Interval = 1 };
        timer.Elapsed += (sender, e) => UpdateDateTime(timeLabel);
        timer.Start();

        // Seleccionar el primer cuadro
        UpdateSelectedTile();

        // Agregar control de teclas
        KeyDown += (sender, e) =>
        {
            if (e.Key == Keys.Left) SelectPreviousTile();
            if (e.Key == Keys.Right) SelectNextTile();
            if (e.Key == Keys.Up) SelectUpTile();
            if (e.Key == Keys.Down) SelectDownTile();
        };
    }

    private StackLayout CreateTopBar(Label timeLabel)
    {
        var topBar = new StackLayout
        {
            Orientation = Orientation.Horizontal,
            Spacing = 20,
            HorizontalContentAlignment = HorizontalAlignment.Stretch,
            Items =
            {
                new StackLayoutItem(null, true),
                new StackLayoutItem(timeLabel, HorizontalAlignment.Right)
            }
        };
        return topBar;
    }

    private void UpdateDateTime(Label timeLabel)
    {
        Application.Instance.Invoke(() =>
        {
            var now = DateTime.Now;
            timeLabel.Text = now.ToString("HH:mm") + "  " + now.ToString("dddd, d/M/yy");
        });
    }

    private StackLayout CreateGameArea(int startIndex, int endIndex)
    {
        var gameArea = new StackLayout
        {
            Orientation = Orientation.Horizontal,
            HorizontalContentAlignment = HorizontalAlignment.Center, // Aseguramos que los cuadros estén centrados
            Spacing = 20,
            Items =
            {
                new StackLayoutItem(gameTiles[startIndex]),
                new StackLayoutItem(gameTiles[startIndex + 1]),
                new StackLayoutItem(gameTiles[startIndex + 2]),
                new StackLayoutItem(gameTiles[startIndex + 3])
            }
        };

        return gameArea;
    }

    private Button CreateGameTile(string text)
    {
        var gameTile = new Button
        {
            Text = text,
            Size = new Size(200, 200),
            BackgroundColor = Colors.Gray,
        };

        return gameTile;
    }

    private Button CreateBottomMenuButton(string text)
    {
        var button = new Button
        {
            Text = text,
            Size = new Size(120, 40),
            BackgroundColor = Colors.Gray
        };

        return button;
    }

    private void SelectPreviousTile()
    {
        if (isBottomMenuSelected)
        {
            if (bottomMenuIndex > 0) bottomMenuIndex--;
            UpdateSelectedBottomMenu();
        }
        else
        {
            if (selectedIndex > 0) selectedIndex--;
            UpdateSelectedTile();
        }
    }

    private void SelectNextTile()
    {
        if (isBottomMenuSelected)
        {
            if (bottomMenuIndex < bottomMenu.Length - 1) bottomMenuIndex++;
            UpdateSelectedBottomMenu();
        }
        else
        {
            if (selectedIndex < gameTiles.Length - 1) selectedIndex++;
            UpdateSelectedTile();
        }
    }

    private void SelectUpTile()
    {
        if (isBottomMenuSelected)
        {
            isBottomMenuSelected = false;
            UpdateSelectedTile();
            UpdateSelectedBottomMenu();
        }
        else
        {
            if (selectedIndex >= 4) selectedIndex -= 4;
            UpdateSelectedTile();
        }
    }

    private void SelectDownTile()
    {
        if (!isBottomMenuSelected)
        {
            isBottomMenuSelected = true;
            UpdateSelectedTile();
            UpdateSelectedBottomMenu();
        }
        else
        {
            if (selectedIndex < 4) selectedIndex += 4;
            UpdateSelectedTile();
        }
    }

    private void UpdateSelectedTile()
    {
        for (int i = 0; i < gameTiles.Length; i++)
        {
            if (i == selectedIndex)
                gameTiles[i].BackgroundColor = Colors.Red; // Cuadro seleccionado
            else
                gameTiles[i].BackgroundColor = Colors.Gray; // Cuadros no seleccionados
        }
    }

    private void UpdateSelectedBottomMenu()
    {
        for (int i = 0; i < bottomMenu.Length; i++)
        {
            if (i == bottomMenuIndex)
                bottomMenu[i].BackgroundColor = Colors.Red; // Botón seleccionado
            else
                bottomMenu[i].BackgroundColor = Colors.Gray; // Botones no seleccionados
        }
    }

    private StackLayout CreateBottomBar()
    {
        var bottomBar = new StackLayout
        {
            Orientation = Orientation.Horizontal,
            Spacing = 20,
            HorizontalContentAlignment = HorizontalAlignment.Stretch,
            VerticalContentAlignment = VerticalAlignment.Bottom, // Aseguramos que los botones estén en la parte inferior
            Items =
            {
                bottomMenu[0],
                bottomMenu[1],
                bottomMenu[2],
                bottomMenu[3],
                bottomMenu[4],
                bottomMenu[5]
            }
        };

        return bottomBar;
    }
}
