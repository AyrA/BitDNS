# BitDNS
Bitmessage DNS integration and namecoin tunnel.

## What?
This tool will allow you to look up bitmessage names, that have been set up according to the [current proposal] (https://bitmessage.org/forum/index.php/topic,2767.0.html) in the bitmessage forum

## Why DNS?
DNS is easy to use and is accessible for almost everyone without third party software.

## You just said "no third party", but ...
The Bitmessage client has only namecoin capability built into it. DNS is not natively supported (yet).

## How to get it?
Just [Download](https://bitmessage.ch/BitDNS.exe) the tool, place it anywhere on your drive and launch it after bitmessage. I will upload the source shortly. (C#)

## How to set it up?
* Start Bitmessage
* Start BitDNS
* Open the Bitmessage settings window, go into the namecoin tab
* Configure Namecoind for localhost and port 8337 (do not use 8336)
* Enter a username and a password. This is optional, if you do not plan on using namecoin at all and only want to use DNS (see next question)
* Click "Test" and you should see a message, that namecoin 0.12.34.56 is running
* Click OK

## I can use namecoin too?
Yes. If you also enter your namecoin API username and password in bitmessage, the BitDNS server will forward all namecoin requests to your namecoind instance and answer all DNS requests too. Namecoind must be running on port 8336 for this to work (this is the default setting).

## What if I do not want namecoin?
Just don't install namecoin then. The client will still try to forward requests to your namecoin instance, but if none is available, it will correctly format an error response for bitmessage. The tool will never ever look up DNS names, if your query does not starts with "DNS/".

## How to use it?
To get it running after the initial setup, just launch it. It places a context menu icon in the tray. The tool can be closed with a double click. There is no user interface.
If you have set it up correctly in bitmessage, you can see a "Fetch Namecoin ID" button on bitmessage.
To fetch a namecoin ID, just enter the name (or id/name) in the field and click the button. To fetch a DNS record, enter "DNS/name", for example "DNS/list.ayra.ch"

## How do I check a DNS name?
You can use [this page](http://home.ayra.ch/dns.php?DNS=list.ayra.ch) to look up bitmessage records and test them if you wish.

Existing records (which can be used as-is) are:
* DNS/ayra.ch
* DNS/bm.ayra.ch
* DNS/list.ayra.ch

## How do I get a DNS name?
You can buy a DNS name for cheap (some providers even accept bitcoin) everywhere in the internet. It costs you usually 5$ to 10$ a year.
If you know somebody who owns a domain, he probably even sets up the record for free for you because it does not causes any troubles with existing services.

## Image
See the attached image for an example configuration.

![Alt text](https://bitmessage.org/forum/index.php?action=dlattach;topic=3281.0;attach=132;image)
