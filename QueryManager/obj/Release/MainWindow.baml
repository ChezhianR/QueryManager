   M S B A M L   `   `   `  ���� G  CQueryManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:QueryManagerQueryManager      QueryManager.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     ,localclr-namespace:QueryManager       � Title$  Query Manager��$	��800��$	��950��.���  � RowDefinitions �� $
��auto ��� $��1* ��� $��40��.+��� $��0��
���   �$��1q�$��Stretch=�$
��auto��$	��Top=� �� CornerRadius$ 8,8,8,8��$
��auto��$��10,10,10,10q�._�� $
��auto�� � ColumnDefinitions �� $��* �$	��250���� $��* �$	��300���� $��* �$	��250���� $��* ��� $��* �$��90�� �� $��10 ��� $��* ��� $��10 ��� $
��Auto �.+��� $��2��$��1��$��Select Company :��$
��Left=�$��	10,10,0,0q�$��Center=�$	��120���� -   $��cmd_companies��$��2�� �� SelectionMode$ Multiple=�$��Right=�$��0,10,0,0q�$��1��$��Center=�$
��Auto��$	��120��$��45��{�.   �� DisplayMemberPath$ CompanyName�� �� SelectedValue$ CompanyNumber������ *	Companies+�� $��0��$��1��$��User Name :��$
��Left=�$��	10,10,0,0q�$��Center=�$	��120���� -   $��	txt_Uname��$��0��$��1��$��Right=�$��23��$��0,10,0,0q� �� TextWrapping$
 Wrap=�$��Center=�$
��auto��$	��120���� $��1��$��1��$��Password��$
��Left=�$��	10,10,0,0q�$��Center=�$��80��G� -   $��txt_password��$��1��$��1��$
��Left=�$��23��$��	90,10,0,0q�$��Center=�$
��auto��$	��120���� -   $��btn_Connect��$��1��$��1��$��Connect��$��Right=�$��
135,10,0,0q�$��Center=�$��75���� -   $��	btn_Query��$��1��$��4��$��Execute Query��$��Center=�$��	12,12,2,4q�$��76��{�.  9� 
���   �$��4��$��5��$��Query Builder��$��Stretch=�$��	10,10,0,0q�$	��Top=�.��� ���	#FFE5E5E5 �� $��25 ��� $��30��$
��Auto � �� $��1* ��� $��1* �.+��� $��0��$��0��$��Enter the Query��$
��Left=��� -   $��txt_qury��$��1��$��2�� �� AcceptsReturn . $
 Wrap=�$��10,0,5,5q�$��Stretch=�"	 �� VerticalScrollBarVisibility$
	 Auto=�$
 �� HorizontalScrollBarVisibility$

 Auto=��� $��1��
���   �$��2q�$ 8,8,8,8��$��10,10,10,10q�._�&   System.Windows.Controls.DataGrid  -   $��	dg_result��$��1,1,1,1q�   AutoGenerateColumns .    
IsReadOnly .    ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$ * ����  � ContextMenu �� .@�m� -   $��Export to Excel���� $��2��$��Stretch=�$��10,1,5,2q����� .R�� $��Stretch=� �� $��1* ��� $
��auto ��� $	��145 �.@��� $��0��.��� -	   $��Prg_bar�� � Maximum$	 100Q� � Minimum$ 0Q�$	��240��$��25��$��Stretch=�$	��Top=��� $��1��.���� �� $��2��.���� -
   $��lbl_sta_text��$��Sample3��