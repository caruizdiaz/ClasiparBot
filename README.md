ClasiparBot
===========

Bot for automatically refresh ads on Clasipar.com.

How it works
============
When you publish an ad on Clasipar.com, it sends you an email for managing your announcement.
This is an example of the email you would receive:

> Gracias por anunciar en Clasipar.com
> 
> A continuación le pasamos algunos enlaces útiles:
> 
> Para ver su anuncio: 
> http://clasipar.paraguay.com/link_to_your_ad.html
> 
> Para administrar sus anuncios:
> http://clasipar.paraguay.com/users/hash_login?hash=sha1_hashed_id

With this email in hand, you will need to extract the following information:

1. Link to your announcement: http://clasipar.paraguay.com/link_to_your_ad.html
2. Link to your management page: http://clasipar.paraguay.com/users/hash_login?hash=sha1_hashed_id
3. The email you had put to submit the announcement.

Having done this, you will have to create a text file which will be supplied to 
the bot. Follow the below syntax:

hash-id;your-email;your-ad;operation

Example:

http://clasipar.paraguay.com/users/hash_login?hash=sha1_hashed_id;my@email.com;http://clasipar.paraguay.com/link_to_your_ad.html;refresh

Finally, pass the path-to-the-file to the bot:

mono ClasiparBot.exe /home/carlos/clasipar.conf

If the program will be running on Linux, you can setup a cron job for refreshing purpose.
For example, if you want to refresh you ad everyday, at 9 am, you can go as follows:

0 9 * * * mono ClasiparBot.exe /home/carlos/clasipar.conf > /tmp/output &

