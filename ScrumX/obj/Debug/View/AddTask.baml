   M S B A M L   `   `   `  ����A  =ScrumX, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:ScrumX.ViewScrumX.View  5   	   U QMahApps.Metro, Version=1.5.0.23, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls 5   	   5clr-namespace:ScrumX.ConvertersScrumX.Converters  5
   	       ScrumX.View.AddTask   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:ScrumX.View     5   	   PControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     5   	   Y UMahApps.Metro.IconPacks.Modern, Version=1.9.1.0, Culture=neutral, PublicKeyToken=null=	iconPacks-http://metro.mahapps.com/winfx/xaml/iconpacks  5   	   U QMaterialDesignThemes.Wpf, Version=1.5.0.523, Culture=neutral, PublicKeyToken=nullX TMaterialDesignThemes.MahApps, Version=0.0.11.0, Culture=neutral, PublicKeyToken=nullHmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes   5	   	   2
convertersclr-namespace:ScrumX.Converters   5
   	   -   5        $��Window��5   8   $��0q�5      (  "MahApps.Metro.Controls.MetroWindow    	GlowBrush
  �   � � 
ResizeMode$ CanResizeWithGrip=�5         MaterialDesignPaper#���   5   	   $��Medium"�5      $��14(�5   	      NonActiveGlowBrush �	#CDFF0000 � WindowStartupLocation$ CenterScreen=�5        PrimaryHueMidBrush   WindowTitleBrush# �  5      $X��Rpack://application:,,,/MaterialDesignThemes.Wpf;component/Resources/Roboto/#Roboto)�5   	    � Title$ Dodaj zadanie��5   	   $	��355��6   $	��300��6,      ShowCloseButton .   � 	Resources 5       �� MergedDictionaries	 �� Source5  /ScrumX.Converters.ProjectToProjectNameConverter  projectToProjectName
 �� 
TargetType �� Property �� Value�� 5   
    5      �� 5      $I	 Cpack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml?�6%   �� 5      $F	 @pack://application:,,,/MahApps.Metro;component/Styles/Fonts.xaml?�6%   �� 5      $G	 Apack://application:,,,/MahApps.Metro;component/Styles/Colors.xaml?�6%   �� 5       $e	 _pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Light.xaml?�6%   �� 5!      $h	 bpack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Defaults.xaml?�6%   �� 5"      $q	 kpack://application:,,,/MaterialDesignThemes.MahApps;component/Themes/MaterialDesignTheme.MahApps.Fonts.xaml?�6%   �� 5#      $s	 mpack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Accent/MaterialDesignColor.Lime.xaml?�6%   %B  &	       '��      '�� [     '�� �       5%      67   �� 5&      "
 ��6   .�5'      ��  � ��$ 10q�6+   5(      �� 5)      "
 ��6   .�5*      ��  � ��$ 10q�6+   5+      �� 5,      "
 ��6   .�5-      ��  � ��$ 	10,6,10,6q�6+   �� 5.       � ��$ Center=�6=   5/      50      .��52      �  � ColumnDefinitions 53   
   �� 54      $
��auto �6   �� 55      $��* �6    � RowDefinitions 57   
   �� 58      $
��auto �6   �� 59      $
��auto �6   �� 5:      $
��auto �6   �� 5;      $
��auto �6   �� 5<      $
��auto �6   �� 5=      $
��auto �6   .+�5?   
   �� $��Tytuł��6   $��	10,6,10,6q�6    �� 5@   
   $��1��6   $��25��6b   ��6"   �� *	TaskTitle+ �� UpdateSourceTrigger$ PropertyChanged=�6b   �� 5A   
   $��Projekt��6   $��1��6"   �� 5B   
   $��1��6   $��1��5C      $��25��5G      *  $MahApps.Metro.Controls.TextBoxHelper   ClearTextButton .    UseFloatingWatermark .    	Watermark$ Priority...��5J       �� 
IsEditable .  �� SelectedItem 5D      �� *TaskProject+ �� Mode$ OneWayToSource=���5E      �� *
Projects+��5L      �� 5M      .R�5N      �� $��10(�65   ��6   �� *Name+65   5O      5Q      �� 5S   
   $��SP��6   $��2��6   �� 5T   
   $��1��6   $��2��5U      $��25��5Y       .  . $ SP...��5\       .  5V      �� *
SelectedSP+��5W      �� *StoryPointValues+��5^      �� 5_      .R�5`      �� $��12(�60   ��6   �� 60   5a      5c      �� 5d   
   $��	Priorytet��6   $��3��6$   �� 5e   
   $��1��5f      $��3��5g      $��25��5k       .  . $ Priority...��5n       .  5h      �� *SelectedPririty+��5i      �� *PriorityValues+��5p      �� 5q      .R�5r      �� $��12(�60   ��6   �� 60   5s      5u      �� 5v   
   $
��Opis��6   $��4��6   $��Center=�6-   �� 5w   
   $��1��6   $��4��6"    �� TextWrapping$
 Wrap=�6P   $��70��6d    �� AcceptsReturn .  �� 
AcceptsTab . ��6/   �� *TaskDescription+6�   � 5x   
   $��2��6   $��5��6#   $��5,5,5,0q�60   $��Center=�6A    5y      �� 5z      $
��auto �6#   �� 5{      $��* �6#   .+�5~      ��  $  MaterialDesignRaisedAccentButton#���  5      $��0q�5�      $��2,2,2,2q�5�      $��Center=�5�      $��Center=�5�      $��Center=�5�      $��Center=�5�      $��0��5�      ��5�      �� *CancleCommand+��5�      ��  �� ElementName$ Window��.��5�      � $��2,2,2,2q�6   $��Center=�5�      $��Center=�5�      $��22��5�       5�      �� 5�      $
��auto �6'   �� 5�      $��* �6'   .+�5�      �� $��0��6   $��Anuluj ��6,   $��16(�5�       $��Center=�5�       $
��Left=�5�       5�      �� 5�      $��1��6   $��Center=�5�      $��Center=�5�      $��Center=�5�      $��Right=�5�        MetroCircleButtonStyle#���  5�      $��0q�6N   .��5�      '  !MaterialDesignThemes.Wpf.PackIcon  $��Center=�62   $��Center=�6O   $��20��6j   $��20��6u   �  �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=1.5.0.523, Culture=neutral, PublicKeyToken=null]]   Kind$ 	ExitToApp=�6�   5�      5�      5�      �� 5�      #���  5�      $��0q�5�      $��2,2,2,2q�5�      $��Center=�5�      $��Center=�5�      $��Center=�5�      $��Center=�5�      $��1��5�      ��5�      �� *AddTaskCommand+��5�      �� $ Window��.��5�      � $��2,2,2,2q�6   $��Center=�5�      $��Center=�5�      $��22��5�       5�      �� 5�      $
��auto �6'   �� 5�      $��* �6'   .+�5�      �� $��0��6   $��16(�5�       $��Center=�5�       $
��Left=�5�       ��5�   ,   �� *AddTaskText+5�      �� 5�      $��1��6   $��Center=�5�      $��Center=�5�      $��Center=�5�      $��Right=�5�      #���  5�      $��0q�6N   .��5�        $��Center=�62   $��Center=�6O   $��20��6j   $��20��6u   $ NotePlusOutline=�6�   5�      5�      5�      5�      5�      5�      