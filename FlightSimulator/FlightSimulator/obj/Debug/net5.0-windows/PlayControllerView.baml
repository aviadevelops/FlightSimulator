   M S B A M L   `   `   `  ����J  FFlightSimulator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:FlightSimulatorFlightSimulator  5   	   H DCircularGauge, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullD2clr-namespace:CircularGauge;assembly=CircularGaugeCircularGauge 5      $    FlightSimulator.PlaybackScreen   5   	   \ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   R NOxyPlot.Wpf, Version=2.0.0.0, Culture=neutral, PublicKeyToken=75e952ba404cdbb0 oxyhttp://oxyplot.org/wpf  5   	   )localclr-namespace:FlightSimulator  5   	   >gauge2clr-namespace:CircularGauge;assembly=CircularGauge  5      -   5          �Name$  window_flighgear��5   	    � Title$ Flight Inspection��5
   	   $	��585��6#   $
��1000��60   ��5      �� 5   
    �� ImageSource$ \Images\plane_bg.jpg��6   6:   .��5      �  � ColumnDefinitions 5   
   �� 5      $��0* �6   �� 5      $��0* �6   �� 5      .+�5   
   �� $��2q�6   
���   �$
��Left=�69   $	��101��6T   $��
33,468,0,0q�6a   $	��Top=�6u   $	��925��6�   $��3��6�   ._�5      �� $��
Play speed��6   $
��Left=�6)   $��28��6D   $��	562,5,0,0q�6P   $	��Top=�6c   $��84��6{   $��14(�6�   $
��Bold"�6�    J� RenderTransformOrigin$ -0.355,0.684&�6�   
�������6�   5      �� 5   
   -   5         ��Name$ btn_open_train��5      $��Load new train flight CSV��6)   $
��Left=�6M   $��24��6h   $��
69,474,0,0q�6t   $	��Top=�6�   $	��127��6�   $
��Bold"�6�   $��10(�6�   $��3��6�   �� 5   
   -   5        $ btn_open_test��5      $��Load new flight CSV��6(   $
��Left=�6F   $��20��6a   $��
69,502,0,0q�6m   $	��Top=�6�   $	��127��6�   $
��Bold"�6�   $��10(�6�   $��3��6�   �� 5   
   -   5        $ btn_back��5      $
��Left=�6#   $��30��6>   $��352,474,0,0q�6J   $	��Top=�6_   $��70��6w   $
��Bold"�6�   $��3��6�   .��5      �� $��\Images\backward.jpg��6   $��Uniform=�62   6F   5      �� 5   
   -   5        $ btn_play��5      $
��Left=�6#   $��30��6>   $��283,474,0,0q�6J   $	��Top=�6_   $��64��6w   $
��Bold"�6�   $��3��6�   .��5       �� $��\Images\play.jpg��6   $��Uniform=�6.   6B   5!      �� 5#   
   -   5        $ 
btn_foward��5#      $
��Left=�6%   $��30��6@   $��509,474,0,0q�6L   $	��Top=�6a   $��70��6y   $
��Bold"�6�   $��2��6�   .��5$      �� $��\Images\forward.jpg��6   $��Uniform=�61   6E   5%      �� 5'   
   -   5        $ 	btn_pause��5'      $
��Left=�6$   $��30��6?   $��431,474,0,0q�6K   $	��Top=�6`   $��70��6x   $
��Bold"�6�   $��3��6�   .��5(      �� $��\Images\pause.jpg��6   $��Uniform=�6/   6C   5)      �� 5+   
   -   5         ��Name$ slider_timesteps��5+   I   $
��Left=�6c   $��22��6~   $��
69,526,0,0q�6�   $	��Top=�6�   $	��857��6�   $
��Bold"�6�   $��14(�6�    �� TickFrequency$ 1Q�6  $��3��6   � Value 6   �� *%#VmPlayController.VM_CurrentTimeStep+6  �� 5,   
   -	   5        	 ��Name$	 label_display_time��5,   J   $
��Left=�6f   $��29��6�   $��808,475,0,0q�6�   $	��Top=�6�   $	��112��6�   
 � Cursor$
  ��6�   $
��Bold"�6�   $ 0.588,0.868&�6�   $��3��6  
���������6   �� *%#VmPlayController.VM_CurrentTimeSpan+6  �� 5-   
   -
   5         ��Name$ txtbox_change_speed��5-      $
��Left=�6/   $��30��6J   $��692,474,0,0q�6V   $�� ��6k    �� TextWrapping$
 Wrap=�6s   $	��Top=�6�   $��84��6�   $
��Bold"�6�   $��3��6  �� 5.   
   $
��Left=�6   $	��180��6/   $��	765,0,0,0q�6<   
���   �$	��Top=�6^   $	��235��6v   $��3��6�   �� 5/   
   -   5        $ slider_rudder��5/   :   $
��Left=�6Q   $��23��6l   $��802,154,0,0q�6x   $	��Top=�6�   $	��172��6�   $ 1Q�6�   $��3��6�    6   �� *VmJoystick.VM_Rudder+6�   �� 50   
   -   5        $ slider_throttle��50   <   $
��Left=�6U   $��23��6p   $��
698,74,0,0q�6|   $	��Top=�6�   $	��164��6�   $ 0.5,0.5&�6�   $ 1Q�6�   $��3��6�    6   �� *VmJoystick.VM_Throttle+z�51      V� 52      .~�53      �� �� 54      �� 55       �� Angle$ 270.334Q�6&   T� 56      57      59      �� 5A   
   $��	-2,4,2,-6q�6   $��3��6$   .+�5B      �� -   5         ��Name$ JoystickBigCanvas��5B      $	��131��60   $	��135��6=    �� Left$	 826��6I   
 �� Top$ 10��6[   .+�5C      A� -   5         A�Name$ CanvasLittleEllipse��5C      
�������$	��131��6C   
���   �$	��138��6_   $ -7��6k   $ 9��6|   �� 5D      -   5        $ JoystickLittleCanvas��5D      $��59��67   $��58��6C   $ 36��6N   $ 36��6_   .+�5E      A� -   5        $ JoystickLittleEllipse��5E      
���   �$��59��6J   
���   �$��58��6e   $ -1��6p   $ 8��6�   z�5F      T� 5G      -   5        EllipseTransform�� T� X 5G   K   �� *VmJoystick.VM_Aileron+ T� Y 6o   �� *VmJoystick.VM_Elevator+5I      5J      5K      �� 5L      -   5         ��Name$ parameterList��5L      $	��463��6\   $	��118��6i   ��6-   �� *VmJoystick.VM_ListBox+6u     OxyPlot.Wpf.Plot  5M         TitleFontSize$ 15Q�6E   $	��181��6X   $	 158��6e   $ 10��6w   $	��230��6�      Title 6   �� *VmJoystick.VM_GraphNameLeft+   Series. 5N        OxyPlot.Wpf.LineSeries  ��6!   �� *VmJoystick.VM_PointsLeft+5O        5P      $ 15Q�6F   $	��181��6Y   $	 460��6f   $ 10��6x   $	��230��6�    6   �� *VmJoystick.VM_GraphNameRight+. 5Q        ��6!   �� *VmJoystick.VM_PointsRight+5R        5S      -   5          Name$ anomaliesGraph��5S      $	��200��6_   $	 146��6l   $	 196��6~   $	��418��6�    6/   �� *VmJoystick.VM_GraphNameBottom+   Axes 5T        OxyPlot.Wpf.LinearAxis  5U        OxyPlot.Wpf.Axis   Position$ Bottom=�6%      TitleFontSize$ 10Q�6�      	TitleFont$ Comic Sans MS��6�      Minimum 67   �� *!VmPlayController.VM_CurrentMinX+   Maximum 6k   �� *!VmPlayController.VM_CurrentMaxX+   Title 6�   �� *VmJoystick.VM_GraphNameLeft+6�     5V      $
 Left=�6%   $ 10Q�6�   $ Comic Sans MS��6�    65   �� *!VmPlayController.VM_CurrentMinY+ 6i   �� *!VmPlayController.VM_CurrentMaxY+ 6�   �� *VmJoystick.VM_GraphNameRight+6�   . 5Y            	LineStyle$
  None=�6W   !   
MarkerType$! Circle=�6h   "   
MarkerSize$" 3Q�6|   #   
MarkerFill$	# Red��6�   ��6!   �� *VmJoystick.VM_PointsBottomRed+6�     5Z      $
  None=�6X   $! Circle=�6i   $" 2Q�6}   $# Black��6�   ��6!   �� * VmJoystick.VM_PointsBottomGray+6�   5\      �� 5]      -   5        $ btn_load_dll��5]      $��	Load DLL ��6+   $��30��6?   $	 208��6K   $	 469��6]   $��66��6n   $
��Bold"�6�   �� 5^      -   5        $ anomalies_lb��5^      
���  ��$��Center=�6=   $��50��6�   $	��178��6�   $	 266��6�   $	 411��6�   ��6a   �� *VmJoystick.VM_ErrorsInGraph+6�   �� 5_      $��Anomalous time steps��6   $��Center=�63   
���   �$	 264��6j   $	 390��6|   $	��178��6�   $��24��6�   $
��Bold"�6�   5`      (� 5a   
   $��771,233,-5,-9q�6   $��3��6)   .��5b      (  "CircularGauge.CircularGaugeControl  $   Radius$$ 65Q�6+   %   ScaleRadius$% 45Q�5c      &   ScaleStartAngle$	& 120Q�5d      '   ScaleSweepAngle$	' 300Q�5e      (   PointerLength$( 23Q�5f      )   PointerCapRadius$) 25Q�5g      *   MajorDivisionsCount$* 10Q�5j      +   MinorDivisionsCount$+ 5Q�5k      ,   RangeIndicatorThickness$, 10Q�5m       -   RangeIndicatorLightRadius$- 15Q�5n      .   ScaleLabelRadius$. 32Q�5o      /   ScaleLabelSize$/ 30,10��5p      0   ScaleLabelFontSize$0 8Q�5q      1   ScaleLabelForeground$1 	LightGray��5r      2   MajorTickSize$
2 10,3��5s      3   MinorTickSize$	3 3,1��5t      4   MajorTickColor$4 	LightGray��5u      5   MinorTickColor$5 White��5v      6   GaugeBackgroundColor$6 Black��5w      7   PointerThickness$7 10Q�5x      8   DialTextOffset$8 40Q�5y      9   DialText$	9 Yaw��5z      $��Comic Sans MS)�5{      :   DialTextColor$: 	LightGray��5|      ;   DialTextFontSize$; 20��5}      $	��208��6#   $	��264��60   <   MinValue< 5h      �� *VmStatDisplay.VM_MinYaw+=   MaxValue= 5i      �� *VmStatDisplay.VM_MaxYaw+>   CurrentValue> 5l      �� *VmStatDisplay.VM_Yaw+5}   0   5~      (� 5   
   $��620,231,103,-16q�6   $��3��6+   .��5�        $$ 65Q�6+   $% 45Q�5�      $	& 120Q�5�      $	' 300Q�5�      $( 23Q�5�      $) 25Q�5�      $* 10Q�5�      $+ 5Q�5�      $, 10Q�5�      $- 15Q�5�      $. 32Q�5�      $/ 30,10��5�      $0 8Q�5�      $1 	LightGray��5�      $
2 10,3��5�      $	3 3,1��5�      $4 	LightGray��5�      $5 White��5�      $6 Black��5�      $7 10Q�5�      $8 40Q�5�      $
9 Roll��5�      $��Comic Sans MS)�5�      $: 	LightGray��5�      $; 20��5�      $	��264��6'   $	��328��64   < 5�      �� *VmStatDisplay.VM_MinRoll+= 5�      �� *VmStatDisplay.VM_MaxRoll+> 5�      �� *VmStatDisplay.VM_Roll+5�   4   5�      (� 5�   
   $��517,233,250,-9q�6   $��3��6*   .��5�        $$ 65Q�6+   $% 45Q�5�      $	& 120Q�5�      $	' 300Q�5�      $( 23Q�5�      $) 25Q�5�      $* 10Q�5�      $+ 5Q�5�      $, 10Q�5�      $- 15Q�5�      $. 32Q�5�      $/ 30,10��5�      $0 8Q�5�      $1 	LightGray��5�      $
2 10,3��5�      $	3 3,1��5�      $4 	LightGray��5�      $5 White��5�      $6 Black��5�      $7 10Q�5�      $8 40Q�5�      $9 Pitch��5�      $��Comic Sans MS)�5�      $: 	LightGray��5�      $; 20��5�      $	��206��6'   $	��276��64   < 5�      �� *VmStatDisplay.VM_MinPitch+= 5�      �� *VmStatDisplay.VM_MaxPitch+> 5�      �� *VmStatDisplay.VM_Pitch+5�   4   5�      (� 5�   
   $��517,111,251,112q�6   $��3��6+   .��5�        $$ 65Q�6+   $% 45Q�5�      $	& 120Q�5�      $	' 300Q�5�      $( 23Q�5�      $) 25Q�5�      $* 10Q�5�      $+ 5Q�5�      $, 10Q�5�      $- 15Q�5�      $. 32Q�5�      $/ 30,10��5�      $0 8Q�5�      $1 	LightGray��5�      $
2 10,3��5�      $	3 3,1��5�      $4 	LightGray��5�      $5 White��5�      $6 Black��5�      $7 10Q�5�      $8 40Q�5�      $
9 Alt.��5�      $��Comic Sans MS)�5�      $: 	LightGray��5�      $; 20��5�      $	��201��6'   $	��271��64   < 5�      �� *VmStatDisplay.VM_MinAlt+= 5�      �� *VmStatDisplay.VM_MaxAlt+> 5�      �� *VmStatDisplay.VM_Altimeter+5�   4   5�      �� 5�   
   $��Rudder��6   $
��Left=�6!   $��32��6<   $��851,175,0,0q�6H   $	��Top=�6]   $��76��6u   $��Comic Sans MS)�6�   $
��Bold"�6�   $��14(�6�   $��3��6�   �� 5�   
   $��Throttle��6   $
��Left=�6#   $��32��6>   $��
712,62,0,0q�6J   $	��Top=�6^   $��81��6v   $ 0.5,0.5&�6�   $��14(�6�   $��Comic Sans MS)�6�   $
��Bold"�6�   $��3��6�   z�5�      V� 5�      .~�5�      �� �� 5�      �� 5�      $ -90.415Q�6&   T� 5�      5�      5�      (� 5�   
   $��641,111,127,112q�6   $��3��6+   .��5�        $$ 65Q�6+   $% 45Q�5�      $	& 120Q�5�      $	' 300Q�5�      $( 23Q�5�      $) 25Q�5�      $* 10Q�5�      $+ 5Q�5�      $, 10Q�5�      $- 15Q�5�      $. 32Q�5�      $/ 30,10��5�      $0 8Q�5�      $1 	LightGray��5�      $
2 10,3��5�      $	3 3,1��5�      $4 	LightGray��5�      $5 White��5�      $6 Black��5�      $7 10Q�5�      $8 40Q�5�      $9 Speed��5�      $��Comic Sans MS)�5�      $: 	LightGray��5�      $; 20��5      $	��201��6'   $	��271��64   < 5�      �� *VmStatDisplay.VM_MinSpeed+= 5�      �� *VmStatDisplay.VM_MaxSpeed+> 5�      �� *VmStatDisplay.VM_Airspeed+5   4   5     (� 5  
   $��773,109,-5,113q�6   $��3��6*   .��5       $$ 65Q�6+   $% 45Q�5     $	& 120Q�5     $	' 300Q�5     $( 23Q�5     $) 25Q�5     $* 10Q�5     $+ 5Q�5     $, 10Q�5     $- 15Q�5     $. 32Q�5     $/ 30,10��5     $0 8Q�5     $1 	LightGray��5     $
2 10,3��5     $	3 3,1��5     $4 	LightGray��5     $5 White��5     $6 Black��5     $7 10Q�5     $8 40Q�5     $
9 Deg.��5     $��Comic Sans MS)�5     $: 	LightGray��5     $; 20��5     $	��201��6'   $	��271��64   < 5	     �� *VmStatDisplay.VM_MinDeg+= 5
     �� *VmStatDisplay.VM_MaxDeg+> 5     �� *VmStatDisplay.VM_Heading_deg+5  4   5     �� 5   
   $��Aileron��6   $
��Left=�6"   $��32��6=   $��
801,43,0,0q�6I   $	��Top=�6]   $��81��6u   $ 
0.43,2.083&�6�   $��14(�6�   $��Comic Sans MS)�6�   $
��Bold"�6�   $��3��6�   z�5!     V� 5"     .~�5#     �� �� 5$     �� 5%     $ 0Q�6&   T� 5&     $ 56.366Q�6)   $ -45.353Q�64   5'     5)     �� 5*  
   $��Elevator��6   $
��Left=�6#   $��32��6>   $��
923,48,0,0q�6J   $	��Top=�6^   $	��105��6v   $ 0.5,0.5&�6�   $��14(�6�   $��Comic Sans MS)�6�   $
��Bold"�6�   $��3��6�   z�5+     V� 5,     .~�5-     �� �� 5.     �� 5/     $	 -90Q�6&   T� 50     51     53     58     59     