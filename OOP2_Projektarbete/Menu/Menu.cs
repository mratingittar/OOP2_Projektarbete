﻿using Skalm.Display;
using Skalm.Input;
using Skalm.Sounds;
using Skalm.Utilities;

namespace Skalm.Menu
{
    internal class Menu
    {
        #region FIELDS
        public readonly string[] title;
        public readonly TreeNode<MenuPage> pages;
        private int pageStartRow;
        private DisplayManager displayManager;
        private SoundManager soundManager;
        private InputManager inputManager;
        public event Action<Page, string>? onMenuExecution;
        #endregion

        #region CONSTRUCTOR
        public Menu(string[] title, TreeNode<MenuPage> pages, DisplayManager displayManager, SoundManager soundManager, InputManager inputManager)
        {
            this.title = title;
            this.pages = pages;
            this.displayManager = displayManager;
            this.soundManager = soundManager;
            this.inputManager = inputManager;
            IsEnabled = false;
            ActivePage = pages.First().Value;
        }
        #endregion

        #region PROPERTIES
        public bool IsEnabled { get; set; }
        public MenuPage ActivePage { get; private set; }
        public int MenuLevel => pages.FindNode(node => node.Value == ActivePage).Depth;
        public int MenuItemIndex { get; set; }
        #endregion

        public void LoadMenu(int titlePadding)
        {
            IsEnabled = true;
            MenuItemIndex = 0;
            displayManager.printer.PrintCenteredInWindow(title, titlePadding);
            pageStartRow = displayManager.windowInfo.CursorPosition.Item2 + titlePadding;
            LoadPage(pages.Value);
        }
        public void LoadPage(MenuPage page, int itemIndex = 0)
        {
            displayManager.eraser.EraseLinesFromTo(pageStartRow, pageStartRow + ActivePage.items.Count + 5); // MAGIC NUMBER; INTRODUCE FIELD OR CONSTANT
            ActivePage = page;
            MenuItemIndex = itemIndex;
            PrintMenu();
        }

        public void ReloadPage()
        {
            LoadPage(ActivePage, MenuItemIndex);
        }

        public void ExecuteSelectedMenuItem()
        {
            string item = ActivePage.items[MenuItemIndex];
            if (item == "Back")
                GoBackOneLevel();
            else if (item == "Exit")
                SendMenuEvent(item);
            else
            {
                if (pages.FindNode(node => node.Value.pageName.ToUpper() == item.ToUpper()) is null)
                    SendMenuEvent(item);
                else
                    LoadPage(pages.FindNode(node => node.Value.pageName.ToUpper() == item.ToUpper()).Value);
            }
        }

        private void SendMenuEvent(string executedItem)
        {
            onMenuExecution?.Invoke(ActivePage.page, executedItem);
        }

        public void Cancel()
        {
            if (MenuLevel > 0)
                GoBackOneLevel();
            else
                SendMenuEvent("Exit");
        }

        public bool MoveMenuUp()
        {
            if (MenuItemIndex == 0)
                return false;

            MenuItemIndex--;
            PrintMenu();
            return true;
        }
        public bool MoveMenuDown()
        {
            if (MenuItemIndex == ActivePage.items.Count - 1)
                return false;

            MenuItemIndex++;
            PrintMenu();
            return true;
        }

        public void GoBackOneLevel()
        {
            LoadPage(pages.FindNode(node => node.Value.pageName.ToUpper() == ActivePage.pageName.ToUpper()).Parent!.Value);
        }

        private void PrintMenu()
        {
            displayManager.printer.PrintCenteredInWindow(TextTools.AddLightBordersToText(ActivePage.pageName), pageStartRow);
            displayManager.printer.PrintCenteredInWindow("", pageStartRow + 3); // MAGIC NUMBER; INTRODUCE FIELD OR CONSTANT
            HighlightSelectedItem(pageStartRow + 4); // MAGIC NUMBER; INTRODUCE FIELD OR CONSTANT
        }

        private void HighlightSelectedItem(int startRow)
        {
            int count = 0;
            foreach (var item in ActivePage.items)
            {
                string pageItem = item.Value;

                if (ActivePage.page is Page.Music && pageItem == soundManager.CurrentlyPlaying.soundName)
                    pageItem = TextTools.AddPointersToString(pageItem, 3);

                if (ActivePage.page is Page.InputMethod && pageItem == inputManager.moveInput.GetType().Name)
                    pageItem = TextTools.AddPointersToString(pageItem, 3);

                if (item.Key == ActivePage.items.Last().Key)
                {
                    displayManager.printer.PrintCenteredInWindow("", startRow + count);
                    count++;
                }

                if (item.Key == MenuItemIndex)
                    displayManager.printer.PrintCenteredInWindow(pageItem, startRow + count, true);
                else
                    displayManager.printer.PrintCenteredInWindow(pageItem, startRow + count);
                count++;
            }
        }
    }
}
