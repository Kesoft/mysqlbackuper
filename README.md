# MySQL Backuper
MySQL database automatic backup service.

## Install

Download installer from [MySQL Backuper 1.0.0.0 Installer](https://github.com/Kesoft/mysqlbackuper/blob/master/installer/MySQL%20Backuper%201.0.0.0.exe) and install step by step.

## Configure

Open config file ( c:\program profile(x86)\kesoft\MySQL Backuper\mysqlbackuper.exe.config), change settings to your settings.

    <?xml version="1.0" encoding="utf-8"?>
	<configuration>
  	<configSections>
    <sectionGroup name="applicationSettings" type="System.Configuration.ApplicationSettingsGroup, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089">
      <section name="Kesoft.MySqlBackuper.Properties.Settings" type="System.Configuration.ClientSettingsSection, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" requirePermission="false" />
    </sectionGroup>
  	</configSections>
  	<startup>
    <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.5"/>
  	</startup>
  	<applicationSettings>
    <Kesoft.MySqlBackuper.Properties.Settings>
      <setting name="DataSource" serializeAs="String">
        <value>localhost</value>
      </setting>
      <setting name="Database" serializeAs="String">
        <value>csm-v4</value>
      </setting>
      <setting name="UserId" serializeAs="String">
        <value>root</value>
      </setting>
      <setting name="Password" serializeAs="String">
        <value>fhit</value>
      </setting>
      <setting name="IntervalHours" serializeAs="String">
        <value>24</value>
      </setting>
      <setting name="BackupPath" serializeAs="String">
        <value>C:\ProgramData\database\</value>
      </setting>
      <setting name="Tables" serializeAs="String">
        <value>users,satellites</value>
      </setting>
    </Kesoft.MySqlBackuper.Properties.Settings>
  	</applicationSettings>
	</configuration>


## Run

Use administrator to run InstallServer.bat to install MySQL Backuper as a windows service.

