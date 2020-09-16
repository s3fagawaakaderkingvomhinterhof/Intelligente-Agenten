import wx
from logic import *


class MyReader(wx.Frame):

    def __init__(self, parent, title):
        super(MyReader, self).__init__(parent, title=title, size=(300,200))

        department_object = get_departments()
        department_names = []
        size = len(department_object)
        for i in range(0,size):
            department_names.append(department_object[i].get('name'))

        panel = wx.Panel(self)
        box = wx.BoxSizer(wx.VERTICAL)
        self.textpanel = wx.TextCtrl(panel, style=wx.TE_MULTILINE|wx.TE_READONLY)
        box.Add(self.textpanel, 0, wx.EXPAND |  wx.ALL, 20)
        cblbl = wx.StaticText(panel, label="choose department", style=wx.ALIGN_CENTRE)

        box.Add(cblbl, 0, wx.EXPAND | wx.ALL, 5)
        self.combo = wx.ComboBox(panel, choices=department_names)

        box.Add(self.combo, 1, wx.EXPAND | wx.ALL, 5)

        box.AddStretchSpacer()
        self.combo.Bind(wx.EVT_COMBOBOX, self.select_choice)

        panel.SetSizer(box)
        self.Centre()
        self.Show()

    def select_choice(self, event):
        departments = get_departments()
        size = len(departments)
        for i in range(0, size):
            if self.combo.GetValue() == departments[i].get('name'):
                print(i)
                self.textpanel.SetValue(string_builder_of(get_news_of(departments[i].get('url'))))
                break


app = wx.App()
MyReader(None, 'Short News Reader')
app.MainLoop()
