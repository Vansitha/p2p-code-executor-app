   M S B A M L   `   `   `  ����H  DDesktopClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:DesktopClientDesktopClient  5   	       DesktopClient.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:DesktopClient     5   	     � Title$  
MainWindow��5   	   $	��450��6   $	��800��6)   .��5	      �  � ColumnDefinitions 5
   
   �� 5      $
��793* �6   �� 5      $��7* �6   .+�5   
   �� -   5         ��Name$ CodeTextBox��5   M    �� AcceptsReturn . " �� VerticalScrollBarVisibility$ Visible=�6'   $
��Left=�6b   $��
11,102,0,0q�6}    �� TextWrapping$
 Wrap=�6�   $	��Top=�6�   $	��390��6�   $	��224��6�   $��Cascadia Code)�6�   �� 5   
   -   5        $ ResultTextBox��5      $ Visible=�6*   $
��Left=�6P   $��405,102,0,0q�6k   $
 Wrap=�6�   $	��Top=�6�   $	��368��6�   $	��224��6�   $��Cascadia Code)�6�   {�.  6�   �� 5   
   -   5         ��Name$ RunJob��5      $��Run Job��6!   $
��Left=�63   $��
10,345,0,0q�6N   $	��Top=�6b   $	��136��6z   $��34��6�   6�   �� 5   
   -   5         ��Name$ CompletedTextBox��5      $��Completed Jobs: 0��6*   $
��Left=�6F   $��
212,11,0,0q�6a   $	��Top=�6u    J� RenderTransformOrigin$ 0.605,0.618&�6�   $	��110��6�   �� 5   
   -   5        $ OngoingTextBox��5      $��Ongoing Jobs: 0��6(   $
��Left=�6B   $��	97,11,0,0q�6]   $	��Top=�6p   $ 0.605,0.618&�6�   $	��110��6�   �� 5   
   $��Executed Result��6   $
��Left=�6*   $��
405,73,0,0q�6E   $	��Top=�6Y   �� 5   
   $��Python Code to Execute��6   $
��Left=�61   $��	11,73,0,0q�6L   $	��Top=�6_   �� 5   
   -   5        $ ConnectedPort��5      $��Connected Port:��6'   $
��Left=�6A   $��
637,10,0,0q�6\   $	��Top=�6p   $ 0.605,0.618&�6�   $	��136��6�   �� 5   
   -   5        $ QueuedTextBox��5      $��	Queued: 0��6'   $
��Left=�6;   $��	10,11,0,0q�6V   $	��Top=�6i   $ 0.605,0.618&�6�   $��82��6�   �� 5   
   -   5        $ InfoBtn��5      $��
Usage Info��6"   $
��Left=�67   $��688,378,0,0q�6R   $	��Top=�6g   $��85��6   6�   5      5      