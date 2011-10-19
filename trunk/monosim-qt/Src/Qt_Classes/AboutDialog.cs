/********************************************************************************
** Form generated from reading ui file 'AboutDialog.ui'
**
** Created: mer ott 19 16:56:14 2011
**      by: Qt User Interface Compiler for C# version 4.7.3
**
** WARNING! All changes made in this file will be lost when recompiling ui file!
********************************************************************************/


using Qyoto;

public class Ui_AboutDialog
{
    public QGridLayout gridLayout;
    public QVBoxLayout vboxLayout;
    public QFrame FrameTop;
    public QGridLayout gridLayout1;
    public QWidget Logo;
    public QVBoxLayout vboxLayout1;
    public QLabel LblName;
    public QLabel LblDesc;
    public QTabWidget tabInfo;
    public QWidget Informations;
    public QGridLayout gridLayout2;
    public QTextEdit TxtInfo;
    public QWidget Components;
    public QGridLayout gridLayout3;
    public QTextEdit TxtThanks;
    public QDialogButtonBox buttonBox;

    public void SetupUi(QDialog AboutDialog)
    {
    if (AboutDialog.ObjectName == "")
        AboutDialog.ObjectName = "AboutDialog";
    QSize Size = new QSize(660, 460);
    Size = Size.ExpandedTo(AboutDialog.MinimumSizeHint());
    AboutDialog.Size = Size;
    AboutDialog.MinimumSize = new QSize(660, 460);
    AboutDialog.WindowIcon = new QIcon(":/main/resources/Images/comex_256.png");
    AboutDialog.Modal = true;
    gridLayout = new QGridLayout(AboutDialog);
    gridLayout.ObjectName = "gridLayout";
    vboxLayout = new QVBoxLayout();
    vboxLayout.ObjectName = "vboxLayout";
    FrameTop = new QFrame(AboutDialog);
    FrameTop.ObjectName = "FrameTop";
    FrameTop.MinimumSize = new QSize(0, 64);
    FrameTop.AutoFillBackground = false;
    FrameTop.FrameShape = QFrame.Shape.StyledPanel;
    FrameTop.FrameShadow = QFrame.Shadow.Raised;
    gridLayout1 = new QGridLayout(FrameTop);
    gridLayout1.ObjectName = "gridLayout1";
    Logo = new QWidget(FrameTop);
    Logo.ObjectName = "Logo";
    Logo.MinimumSize = new QSize(48, 48);
    Logo.MaximumSize = new QSize(48, 48);
    Logo.StyleSheet = "background-image: url(:/main/resources/monosim_48.png);";

    gridLayout1.AddWidget(Logo, 0, 0, 1, 1);

    vboxLayout1 = new QVBoxLayout();
    vboxLayout1.ObjectName = "vboxLayout1";
    LblName = new QLabel(FrameTop);
    LblName.ObjectName = "LblName";
    QFont font = new QFont();
    font.SetBold(true);
    font.SetWeight(75);
    LblName.Font = font;
    LblName.Margin = 1;

    vboxLayout1.AddWidget(LblName);

    LblDesc = new QLabel(FrameTop);
    LblDesc.ObjectName = "LblDesc";
    LblDesc.Margin = 1;

    vboxLayout1.AddWidget(LblDesc);


    gridLayout1.AddLayout(vboxLayout1, 0, 1, 1, 1);


    vboxLayout.AddWidget(FrameTop);

    tabInfo = new QTabWidget(AboutDialog);
    tabInfo.ObjectName = "tabInfo";
    tabInfo.AutoFillBackground = false;
    Informations = new QWidget();
    Informations.ObjectName = "Informations";
    gridLayout2 = new QGridLayout(Informations);
    gridLayout2.ObjectName = "gridLayout2";
    TxtInfo = new QTextEdit(Informations);
    TxtInfo.ObjectName = "TxtInfo";
    TxtInfo.lineWrapMode = QTextEdit.LineWrapMode.NoWrap;
    TxtInfo.ReadOnly = true;

    gridLayout2.AddWidget(TxtInfo, 0, 0, 1, 1);

    tabInfo.AddTab(Informations, QApplication.Translate("AboutDialog", "Informazioni su", null, QApplication.Encoding.UnicodeUTF8));
    Components = new QWidget();
    Components.ObjectName = "Components";
    gridLayout3 = new QGridLayout(Components);
    gridLayout3.ObjectName = "gridLayout3";
    TxtThanks = new QTextEdit(Components);
    TxtThanks.ObjectName = "TxtThanks";
    TxtThanks.ReadOnly = true;

    gridLayout3.AddWidget(TxtThanks, 0, 0, 1, 1);

    tabInfo.AddTab(Components, QApplication.Translate("AboutDialog", "Componenti", null, QApplication.Encoding.UnicodeUTF8));

    vboxLayout.AddWidget(tabInfo);

    buttonBox = new QDialogButtonBox(AboutDialog);
    buttonBox.ObjectName = "buttonBox";
    buttonBox.StandardButtons = Qyoto.Qyoto.GetCPPEnumValue("QDialogButtonBox", "Ok");

    vboxLayout.AddWidget(buttonBox);


    gridLayout.AddLayout(vboxLayout, 0, 0, 1, 1);


    RetranslateUi(AboutDialog);

    tabInfo.CurrentIndex = 0;


    QMetaObject.ConnectSlotsByName(AboutDialog);
    } // SetupUi

    public void RetranslateUi(QDialog AboutDialog)
    {
    AboutDialog.WindowTitle = QApplication.Translate("AboutDialog", "Dialog", null, QApplication.Encoding.UnicodeUTF8);
    FrameTop.StyleSheet = QApplication.Translate("AboutDialog", "background-color: rgb(255, 255, 255);", null, QApplication.Encoding.UnicodeUTF8);
    tabInfo.SetTabText(tabInfo.IndexOf(Informations), QApplication.Translate("AboutDialog", "Informazioni su", null, QApplication.Encoding.UnicodeUTF8));
    tabInfo.SetTabText(tabInfo.IndexOf(Components), QApplication.Translate("AboutDialog", "Componenti", null, QApplication.Encoding.UnicodeUTF8));
    } // RetranslateUi

}

namespace Ui {
    public class AboutDialog : Ui_AboutDialog {}
} // namespace Ui

