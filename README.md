# ColorConverter
Small utility to auto-generate ARGB C# code from HEX colors.

Input HEX code either with or without leading hash (#), however there is no correction or detection for incorrect input.

6-digit HEX codes will default to an alpha (transparency) component of 255; 
i.e. #FF0000 (RED) will be (255, 255, 0, 0).

8-digit HEX codes will respect the leading two digits as containing the alpha (transparency) component.
Note this is the LEADING digits as this only is designed for ARGB, not RGBA mode HEX values. (Might add a toggle in the future.)
i.e. #AA00FF00 will be (170, 0 , 255, 0). (GREEN with Alpha of 170/255).

Using the "convert button" will preview the converted color in a floating UI box, and generate the needed code in the textbox. 

![Screenshot of ColorConverter showing version 0.1 UI](https://prnt.sc/12a8lz6)