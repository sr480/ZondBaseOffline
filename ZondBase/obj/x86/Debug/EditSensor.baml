   M S B A M L   `   `   `  ����  ZondBase    ZondBase.EditSensor   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � WindowStyle$  
ToolWindow=�5   	    � Title$ Создать��5   	   $	��280��6   $	��340��6&    � 
ResizeMode$ NoResize=�62    � ShowInTaskbar .   � Topmost .  � WindowStartupLocation$ CenterScreen=�6m   .��5      �  � RowDefinitions 5   
   �� 5	      $��* �6   �� 5
      $��28 �6   .+�5   
   �� .+�5      �� $��Номер:��6%   $
��Bold"�65   $��4q�6G   .��5      �� $��Normal"�6@   $��4q�6T    �� SelectedItem 6   �� *SensorIndex+��6_   �� *SensorIndexes+5      �� 5      $��Имя:��6%   $
��Bold"�63   $��4q�6E   .��5      �� $��Normal"�6U   $��4q�6i   ��6   �� *Name+ �� UpdateSourceTrigger$ PropertyChanged=�6i   5      �� 5      $;��5Проектная глубина залегания:��6%   $
��Bold"�6K   $��4q�6]   .��5      �� $��Normal"�6_   $��4q�6s   ��6   �� *ActualPosition+$ PropertyChanged=�6s   5      �� 5      $ ��Номер магнита:��6%   $
��Bold"�6=   $��4q�6O   .��5      �� $��Normal"�6Y   $��4q�6m   ��6   �� *
SensorID+$ PropertyChanged=�6m   5      5      �� 5   
   	 �� Orientation$	 
Horizontal=�6   $��1��6.   $��Right=�6;   .+�5      �� $��80��6,   $��2q�67   ��6   �� *Ok+.��6B   Ok6F   �� 5      $��80��60   $��2q�6;   ��6   �� *Cancel+.��6F   Отмена6N   5      5      5      