   M S B A M L   `   `   `  ����A  =ScrumX, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:ScrumX.ViewScrumX.View  5   	   3clr-namespace:ScrumX.ViewModelScrumX.ViewModel  5      U QMahApps.Metro, Version=1.5.0.23, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls 5	   	   5clr-namespace:ScrumX.ConvertersScrumX.Converters  5   	   ;"clr-namespace:ScrumX.HelperClassesScrumX.HelperClasses  5   	       ScrumX.View.JobDetails   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:ScrumX.View     5   	   4	ViewModelclr-namespace:ScrumX.ViewModel     5      PControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     5	   	   Y UMahApps.Metro.IconPacks.Modern, Version=1.9.1.0, Culture=neutral, PublicKeyToken=null=	iconPacks-http://metro.mahapps.com/winfx/xaml/iconpacks  5
   	   U QMaterialDesignThemes.Wpf, Version=1.5.0.523, Culture=neutral, PublicKeyToken=nullX TMaterialDesignThemes.MahApps, Version=0.0.11.0, Culture=neutral, PublicKeyToken=nullHmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes   5   	   2
convertersclr-namespace:ScrumX.Converters   5   	   1helper"clr-namespace:ScrumX.HelperClasses   5   	      MaterialDesignPaper#���   5   	   $��0q�5      $��Medium"�5      $��14(�5   	   $X��Rpack://application:,,,/MaterialDesignThemes.Wpf;component/Resources/Roboto/#Roboto)�5   	   $	��300��5   	   $	��300��6     � 	Resources  5       �� MergedDictionaries �� Source5  /ScrumX.Converters.ProjectToProjectNameConverter  projectToProjectName/  )ScrumX.Converters.UserToUserNameConverter  userToUserNameConverter,  &ScrumX.Converters.IntToDoubleConverter  intToDoubleConverter'   !ScrumX.HelperClasses.BindingProxy 	 proxy   Data�� 5   
    5   
   �� 5      $I Cpack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml?�6!   �� 5      $F @pack://application:,,,/MahApps.Metro;component/Styles/Fonts.xaml?�6!   �� 5      $G Apack://application:,,,/MahApps.Metro;component/Styles/Colors.xaml?�6!   �� 5      $M Gpack://application:,,,/MahApps.Metro;component/Styles/Accents/Lime.xaml?�6!   �� 5      $e _pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Light.xaml?�6!   �� 5      $h bpack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Defaults.xaml?�6!   �� 5       $q kpack://application:,,,/MaterialDesignThemes.MahApps;component/Themes/MaterialDesignTheme.MahApps.Fonts.xaml?�6!   �� 5!      $s mpack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Accent/MaterialDesignColor.Lime.xaml?�6!   %}   &	       &	      &	 &     &	 9       5#      67     5$      61     5%      6.     5&       60   �� 5'      .��5*      �  � DataContext 6   �� *SelectedJob+  5+   
    �� 
TargetType �� Property �� Value �� Triggers	 �� Binding
 �� Source �� Value��%�  '��       7  '�� "    7  '�� D    7  �� 5,      " ��6   .�5-      ��  � ��$ 2q�6+   �� 5.       � ��$ Center=�6=   �� 5/       � y�$ 	Collapsed=�6/    50      �� 51      	 6"   �� *Data.IsDescVisible+8
    True6h   .a�52      ��  � y�$ Visible=�67   53      55      �� 56      " ��6   .�57      ��  � ��$ 2q�6+   �� 58       � ��$ Center=�66   �� 59       � y�$ 	Collapsed=�6/    5:      �� 5;      	 6"   �� *Data.IsDescVisible+8
    True6h   .a�5<      ��  � y�$ Visible=�67   5=      5?      �� 5@      " ��6   .�5A      ��  � ��$ 2q�6+   �� 5B       � ��$ Center=�66   �� 5C       � y�$ 	Collapsed=�6/    5D      �� 5E      	 6"   �� *Data.IsDescVisible+8
    True6h   .a�5F      ��  � y�$ Visible=�67   5G      5I      5J       � ColumnDefinitions 5K   
   �� 5L      $
��auto �6   �� 5M      $��* �6    � RowDefinitions 5O   
   �� 5P      $
��auto �6   �� 5R      $
��auto �6   �� 5T      $
��auto �6   �� 5V      $
��auto �6   �� 5X      $
��auto �6   �� 5Z      $
��auto �6   �� 5\      $��* �6   .+�5_   
   �� $��Title��6   �� 5`   
   $��1��68   ��6   �� *Title+ �� Mode$ OneWay=�68   �� 5a   
   $��Story Points��6   $��1��6'   �� 5b   
   $��1��65   $��1��6E   ��6   �� *SP+$ OneWay=�6E   �� 5c   
   $��Priority��6   $��2��6#   �� 5d   
   $��1��6.   $��2��6>   ��6   �� *
Priority+6>   �� 5e   
   $��Project��6   $��3��6"   �� 5f   
   $��1��6^   $��3��6n   ��6   �� *	Project+ �� 	Converter# [ 6n   �� 5g   
   $��
Created by��6   $��4��6%   �� 5h   
   $��1��6^   $��4��6n   ��6   �� *User+# [ 6n   �� 5i   
   $��Description��6   $��5��6&   �� 5j   
   $��1��6*   $��5��6:   ��6   �� *Desc+6:   �� 5k   
   $��6��6   $��2��6"   .+�5l      �� $
��Logs��6   $��Center=�6#   �� 5m      $	��130��6>   ��6   �� *JobsHistoryJob+��5n      �� 5o      .R�5p      �� ��6    �� *	Comment+5q      5s      5t      5u      5w      