   M S B A M L   `   `   `  ����  ZondBase    ZondBase.EditHistoryItem   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � WindowStyle$  
ToolWindow=�5   	    � Title$ Создать��5   	   $	��280��6   $	��340��6&    � 
ResizeMode$ NoResize=�62    � ShowInTaskbar .   � Topmost .  � WindowStartupLocation$ CenterScreen=�6m   .��5      �  � RowDefinitions 5   
   �� 5	      $��* �6   �� 5
      $��28 �6   .+�5   
   �� .+�5      �� $��	Дата:��6%   $
��Bold"�64   $��4q�6F   .��5      (  "System.Windows.Controls.DatePicker  $��Normal"�6;   $��4q�6O      SelectedDate 6   �� *Date+6O   5      �� 5      $��Датчик1:��6%   $
��Bold"�67   $��4q�6I   .��5      �� $��Normal"�6]   $��4q�6q   ��6   �� *Sensor1Value+ �� UpdateSourceTrigger$ PropertyChanged=�6q   5      �� 5      $��Датчик2:��6%   $
��Bold"�67   $��4q�6I   .��5      �� $��Normal"�6]   $��4q�6q   ��6   �� *Sensor2Value+$ PropertyChanged=�6q   5      �� 5      $��Температура:��6%   $
��Bold"�6;   $��4q�6M   .��5      �� $��Normal"�6\   $��4q�6p   ��6   �� *Temperature+$ PropertyChanged=�6p   5      5      �� 5   
   	 �� Orientation$	 
Horizontal=�6   $��1��6.   $��Right=�6;   .+�5      �� $��80��6,   $��2q�67   ��6   �� *Ok+.��6B   Ok6F   �� 5      $��80��60   $��2q�6;   ��6   �� *Cancel+.��6F   Отмена6N   5      5      5      