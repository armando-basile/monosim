/********************************************************************************
** Form generated from reading ui file 'MainWindow.ui'
**
** Created: gio ott 20 14:29:38 2011
**      by: Qt User Interface Compiler for C# version 4.7.3
**
** WARNING! All changes made in this file will be lost when recompiling ui file!
********************************************************************************/


using Qyoto;

public class Ui_MainWindow
{
    public QAction MenuFileNew;
    public QAction MenuFileOpen;
    public QAction MenuFileSaveFile;
    public QAction MenuFileSaveSim;
    public QAction MenuFileClose;
    public QAction MenuFileSettings;
    public QAction MenuFileExit;
    public QAction MenuSimConnect;
    public QAction MenuSimPin;
    public QAction MenuSimSaveFile;
    public QAction MenuSimSaveSim;
    public QAction MenuSimDeleteAll;
    public QAction MenuSimDisconnect;
    public QAction MenuAboutInfo;
    public QWidget centralwidget;
    public QGridLayout gridLayout;
    public QSplitter splitter;
    public QGroupBox FrameFile;
    public QGridLayout gridLayout1;
    public QTreeWidget LstFileContacts;
    public QGroupBox FrameSim;
    public QGridLayout gridLayout2;
    public QTreeWidget LstSimContacts;
    public QMenuBar MainMenu;
    public QMenu MenuFileItem;
    public QMenu MenuReaderItem;
    public QMenu MenuAboutItem;
    public QMenu MenuSimItem;
    public QStatusBar StatusBar;
    public QToolBar TopToolBar;

    public void SetupUi(QMainWindow MainWindow)
    {
    if (MainWindow.ObjectName == "")
        MainWindow.ObjectName = "MainWindow";
    QSize Size = new QSize(631, 570);
    Size = Size.ExpandedTo(MainWindow.MinimumSizeHint());
    MainWindow.Size = Size;
    MainWindow.MinimumSize = new QSize(600, 550);
    MainWindow.WindowIcon = new QIcon(":/main/resources/monosim_128.png");
    MenuFileNew = new QAction(MainWindow);
    MenuFileNew.ObjectName = "MenuFileNew";
    MenuFileNew.icon = new QIcon(":/toolbar/resources/qt/document-new.png");
    MenuFileOpen = new QAction(MainWindow);
    MenuFileOpen.ObjectName = "MenuFileOpen";
    MenuFileOpen.icon = new QIcon(":/toolbar/resources/qt/document-open.png");
    MenuFileSaveFile = new QAction(MainWindow);
    MenuFileSaveFile.ObjectName = "MenuFileSaveFile";
    MenuFileSaveFile.Enabled = false;
    MenuFileSaveFile.icon = new QIcon(":/toolbar/resources/qt/document-save.png");
    MenuFileSaveSim = new QAction(MainWindow);
    MenuFileSaveSim.ObjectName = "MenuFileSaveSim";
    MenuFileSaveSim.Enabled = false;
    MenuFileSaveSim.icon = new QIcon(":/main/resources/chip_32.png");
    MenuFileClose = new QAction(MainWindow);
    MenuFileClose.ObjectName = "MenuFileClose";
    MenuFileClose.Enabled = false;
    MenuFileClose.icon = new QIcon(":/toolbar/resources/qt/document-close.png");
    MenuFileSettings = new QAction(MainWindow);
    MenuFileSettings.ObjectName = "MenuFileSettings";
    MenuFileSettings.icon = new QIcon(":/toolbar/resources/qt/configure.png");
    MenuFileExit = new QAction(MainWindow);
    MenuFileExit.ObjectName = "MenuFileExit";
    MenuFileExit.icon = new QIcon(":/toolbar/resources/qt/application-exit.png");
    MenuSimConnect = new QAction(MainWindow);
    MenuSimConnect.ObjectName = "MenuSimConnect";
    MenuSimConnect.icon = new QIcon(":/toolbar/resources/qt/network-connect.png");
    MenuSimPin = new QAction(MainWindow);
    MenuSimPin.ObjectName = "MenuSimPin";
    MenuSimPin.Enabled = false;
    MenuSimPin.icon = new QIcon(":/toolbar/resources/qt/document-encrypt.png");
    MenuSimSaveFile = new QAction(MainWindow);
    MenuSimSaveFile.ObjectName = "MenuSimSaveFile";
    MenuSimSaveFile.Enabled = false;
    MenuSimSaveFile.icon = new QIcon(":/toolbar/resources/qt/document-save.png");
    MenuSimSaveSim = new QAction(MainWindow);
    MenuSimSaveSim.ObjectName = "MenuSimSaveSim";
    MenuSimSaveSim.Enabled = false;
    MenuSimSaveSim.icon = new QIcon(":/main/resources/chip_32.png");
    MenuSimDeleteAll = new QAction(MainWindow);
    MenuSimDeleteAll.ObjectName = "MenuSimDeleteAll";
    MenuSimDeleteAll.Enabled = false;
    MenuSimDeleteAll.icon = new QIcon(":/toolbar/resources/qt/edit-delete.png");
    MenuSimDisconnect = new QAction(MainWindow);
    MenuSimDisconnect.ObjectName = "MenuSimDisconnect";
    MenuSimDisconnect.Enabled = false;
    MenuSimDisconnect.icon = new QIcon(":/toolbar/resources/qt/network-disconnect.png");
    MenuAboutInfo = new QAction(MainWindow);
    MenuAboutInfo.ObjectName = "MenuAboutInfo";
    MenuAboutInfo.icon = new QIcon(":/toolbar/resources/qt/dialog-information.png");
    centralwidget = new QWidget(MainWindow);
    centralwidget.ObjectName = "centralwidget";
    gridLayout = new QGridLayout(centralwidget);
    gridLayout.ObjectName = "gridLayout";
    splitter = new QSplitter(centralwidget);
    splitter.ObjectName = "splitter";
    splitter.Orientation = Qt.Orientation.Vertical;
    splitter.ChildrenCollapsible = false;
    FrameFile = new QGroupBox(splitter);
    FrameFile.ObjectName = "FrameFile";
    FrameFile.MinimumSize = new QSize(0, 100);
    gridLayout1 = new QGridLayout(FrameFile);
    gridLayout1.ObjectName = "gridLayout1";
    LstFileContacts = new QTreeWidget(FrameFile);
    LstFileContacts.ObjectName = "LstFileContacts";
    LstFileContacts.Enabled = false;
    LstFileContacts.EditTriggers = Qyoto.Qyoto.GetCPPEnumValue("QAbstractItemView", "NoEditTriggers");
    LstFileContacts.selectionMode = QAbstractItemView.SelectionMode.ExtendedSelection;
    LstFileContacts.ItemsExpandable = false;
    LstFileContacts.ExpandsOnDoubleClick = false;

    gridLayout1.AddWidget(LstFileContacts, 0, 0, 1, 1);

    splitter.AddWidget(FrameFile);
    FrameSim = new QGroupBox(splitter);
    FrameSim.ObjectName = "FrameSim";
    FrameSim.MinimumSize = new QSize(0, 100);
    gridLayout2 = new QGridLayout(FrameSim);
    gridLayout2.ObjectName = "gridLayout2";
    LstSimContacts = new QTreeWidget(FrameSim);
    LstSimContacts.ObjectName = "LstSimContacts";
    LstSimContacts.Enabled = false;
    LstSimContacts.EditTriggers = Qyoto.Qyoto.GetCPPEnumValue("QAbstractItemView", "NoEditTriggers");
    LstSimContacts.selectionMode = QAbstractItemView.SelectionMode.ExtendedSelection;
    LstSimContacts.ItemsExpandable = false;
    LstSimContacts.ExpandsOnDoubleClick = false;

    gridLayout2.AddWidget(LstSimContacts, 0, 0, 1, 1);

    splitter.AddWidget(FrameSim);

    gridLayout.AddWidget(splitter, 0, 0, 1, 1);

    MainWindow.SetCentralWidget(centralwidget);
    MainMenu = new QMenuBar(MainWindow);
    MainMenu.ObjectName = "MainMenu";
    MainMenu.Geometry = new QRect(0, 0, 631, 24);
    MenuFileItem = new QMenu(MainMenu);
    MenuFileItem.ObjectName = "MenuFileItem";
    MenuReaderItem = new QMenu(MainMenu);
    MenuReaderItem.ObjectName = "MenuReaderItem";
    MenuAboutItem = new QMenu(MainMenu);
    MenuAboutItem.ObjectName = "MenuAboutItem";
    MenuSimItem = new QMenu(MainMenu);
    MenuSimItem.ObjectName = "MenuSimItem";
    MainWindow.SetMenuBar(MainMenu);
    StatusBar = new QStatusBar(MainWindow);
    StatusBar.ObjectName = "StatusBar";
    MainWindow.SetStatusBar(StatusBar);
    TopToolBar = new QToolBar(MainWindow);
    TopToolBar.ObjectName = "TopToolBar";
    TopToolBar.Movable = false;
    TopToolBar.Floatable = false;
    MainWindow.AddToolBar(Qt.ToolBarArea.TopToolBarArea, TopToolBar);

    MainMenu.AddAction(MenuFileItem.MenuAction());
    MainMenu.AddAction(MenuReaderItem.MenuAction());
    MainMenu.AddAction(MenuSimItem.MenuAction());
    MainMenu.AddAction(MenuAboutItem.MenuAction());
    MenuFileItem.AddAction(MenuFileNew);
    MenuFileItem.AddAction(MenuFileOpen);
    MenuFileItem.AddAction(MenuFileSaveFile);
    MenuFileItem.AddAction(MenuFileSaveSim);
    MenuFileItem.AddAction(MenuFileClose);
    MenuFileItem.AddSeparator();
    MenuFileItem.AddAction(MenuFileSettings);
    MenuFileItem.AddSeparator();
    MenuFileItem.AddAction(MenuFileExit);
    MenuAboutItem.AddAction(MenuAboutInfo);
    MenuSimItem.AddAction(MenuSimConnect);
    MenuSimItem.AddAction(MenuSimPin);
    MenuSimItem.AddSeparator();
    MenuSimItem.AddAction(MenuSimSaveFile);
    MenuSimItem.AddAction(MenuSimSaveSim);
    MenuSimItem.AddAction(MenuSimDeleteAll);
    MenuSimItem.AddSeparator();
    MenuSimItem.AddAction(MenuSimDisconnect);
    TopToolBar.AddAction(MenuFileNew);
    TopToolBar.AddAction(MenuFileOpen);
    TopToolBar.AddAction(MenuFileSaveFile);
    TopToolBar.AddAction(MenuFileSaveSim);
    TopToolBar.AddAction(MenuFileClose);
    TopToolBar.AddSeparator();
    TopToolBar.AddAction(MenuFileSettings);
    TopToolBar.AddAction(MenuSimConnect);
    TopToolBar.AddAction(MenuSimPin);
    TopToolBar.AddAction(MenuSimSaveFile);
    TopToolBar.AddAction(MenuSimSaveSim);
    TopToolBar.AddAction(MenuSimDisconnect);
    TopToolBar.AddSeparator();
    TopToolBar.AddAction(MenuAboutInfo);
    TopToolBar.AddAction(MenuFileExit);

    RetranslateUi(MainWindow);

    QMetaObject.ConnectSlotsByName(MainWindow);
    } // SetupUi

    public void RetranslateUi(QMainWindow MainWindow)
    {
    MainWindow.WindowTitle = QApplication.Translate("MainWindow", "MainWindow", null, QApplication.Encoding.UnicodeUTF8);
    MenuFileNew.Text = QApplication.Translate("MainWindow", "New", null, QApplication.Encoding.UnicodeUTF8);
    MenuFileOpen.Text = QApplication.Translate("MainWindow", "Open", null, QApplication.Encoding.UnicodeUTF8);
    MenuFileOpen.Shortcut = QApplication.Translate("MainWindow", "Ctrl+O", null, QApplication.Encoding.UnicodeUTF8);
    MenuFileSaveFile.Text = QApplication.Translate("MainWindow", "SaveFile", null, QApplication.Encoding.UnicodeUTF8);
    MenuFileSaveSim.Text = QApplication.Translate("MainWindow", "SaveSim", null, QApplication.Encoding.UnicodeUTF8);
    MenuFileClose.Text = QApplication.Translate("MainWindow", "Close", null, QApplication.Encoding.UnicodeUTF8);
    MenuFileClose.Shortcut = QApplication.Translate("MainWindow", "Ctrl+C", null, QApplication.Encoding.UnicodeUTF8);
    MenuFileSettings.Text = QApplication.Translate("MainWindow", "Settings", null, QApplication.Encoding.UnicodeUTF8);
    MenuFileExit.Text = QApplication.Translate("MainWindow", "Exit", null, QApplication.Encoding.UnicodeUTF8);
    MenuFileExit.Shortcut = QApplication.Translate("MainWindow", "Ctrl+Q", null, QApplication.Encoding.UnicodeUTF8);
    MenuSimConnect.Text = QApplication.Translate("MainWindow", "Connect", null, QApplication.Encoding.UnicodeUTF8);
    MenuSimPin.Text = QApplication.Translate("MainWindow", "Pin", null, QApplication.Encoding.UnicodeUTF8);
    MenuSimSaveFile.Text = QApplication.Translate("MainWindow", "SaveFile", null, QApplication.Encoding.UnicodeUTF8);
    MenuSimSaveSim.Text = QApplication.Translate("MainWindow", "SaveSim", null, QApplication.Encoding.UnicodeUTF8);
    MenuSimDeleteAll.Text = QApplication.Translate("MainWindow", "DeleteAll", null, QApplication.Encoding.UnicodeUTF8);
    MenuSimDisconnect.Text = QApplication.Translate("MainWindow", "Disconnect", null, QApplication.Encoding.UnicodeUTF8);
    MenuAboutInfo.Text = QApplication.Translate("MainWindow", "Info", null, QApplication.Encoding.UnicodeUTF8);
    MenuAboutInfo.Shortcut = QApplication.Translate("MainWindow", "Ctrl+I", null, QApplication.Encoding.UnicodeUTF8);
    FrameFile.Title = QApplication.Translate("MainWindow", "Contatti File", null, QApplication.Encoding.UnicodeUTF8);
    LstFileContacts.HeaderItem().SetText(0, QApplication.Translate("MainWindow", "1", null, QApplication.Encoding.UnicodeUTF8));
    LstFileContacts.HeaderItem().SetText(1, QApplication.Translate("MainWindow", "New Column", null, QApplication.Encoding.UnicodeUTF8));
    LstFileContacts.HeaderItem().SetText(2, QApplication.Translate("MainWindow", "Phone Number", null, QApplication.Encoding.UnicodeUTF8));
    FrameSim.Title = QApplication.Translate("MainWindow", "Contatti Sim", null, QApplication.Encoding.UnicodeUTF8);
    LstSimContacts.HeaderItem().SetText(0, QApplication.Translate("MainWindow", "1", null, QApplication.Encoding.UnicodeUTF8));
    LstSimContacts.HeaderItem().SetText(1, QApplication.Translate("MainWindow", "Description", null, QApplication.Encoding.UnicodeUTF8));
    LstSimContacts.HeaderItem().SetText(2, QApplication.Translate("MainWindow", "Phone Number", null, QApplication.Encoding.UnicodeUTF8));
    MenuFileItem.Title = QApplication.Translate("MainWindow", "&File", null, QApplication.Encoding.UnicodeUTF8);
    MenuReaderItem.Title = QApplication.Translate("MainWindow", "&Lettore", null, QApplication.Encoding.UnicodeUTF8);
    MenuAboutItem.Title = QApplication.Translate("MainWindow", "&Aiuto", null, QApplication.Encoding.UnicodeUTF8);
    MenuSimItem.Title = QApplication.Translate("MainWindow", "Sim", null, QApplication.Encoding.UnicodeUTF8);
    TopToolBar.WindowTitle = QApplication.Translate("MainWindow", "toolBar", null, QApplication.Encoding.UnicodeUTF8);
    } // RetranslateUi

}

namespace Ui {
    public class MainWindow : Ui_MainWindow {}
} // namespace Ui

