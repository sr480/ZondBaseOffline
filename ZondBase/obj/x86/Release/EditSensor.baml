   M S B A M L   `   `   `  ����   ZondBase    ZondBase.EditSensor   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � WindowStyle$  
ToolWindow=� � Title$ Создать��$	��280��$	��340�� � 
ResizeMode$ NoResize=� � ShowInTaskbar .   � Topmost .  � WindowStartupLocation$ CenterScreen=�.���  � RowDefinitions �� $��* ��� $��28 �.+��� .+��� $��Номер:��$
��Bold"�$��4q�.���� $��Normal"�$��4q� �� SelectedItem �� *SensorIndex+���� *SensorIndexes+�� $��Имя:��$
��Bold"�$��4q�.���� $��Normal"�$��4q����� *Name+ �� UpdateSourceTrigger$ PropertyChanged=��� $;��5Проектная глубина залегания:��$
��Bold"�$��4q�.���� $��Normal"�$��4q����� *ActualPosition+$ PropertyChanged=��� $ ��Номер магнита:��$
��Bold"�$��4q�.���� $��Normal"�$��4q����� *
SensorID+$ PropertyChanged=��� 	 �� Orientation$	 
Horizontal=�$��1��$��Right=�.+��� $��80��$��2q����� *Ok+.��Ok�� $��80��$��2q����� *Cancel+.��Отмена