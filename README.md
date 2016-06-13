# VeOTP

A simple OTP example. I've created a library named VeOTP.Common that can be used to generate Time-Based One Time Passwords (TOPT).

In the implementation of this I followed guidance in [rfc6238](https://tools.ietf.org/html/rfc6238). 
I have not attempted to provide any of the functionality for clock drift or re-synchronisation.

I've implemented some tests to give coverage and test the assumptions I made in implementation and they do provide coverage on this library. I have not tested any implementation details and only test the public method provided by this library.

To demonstrate usage of the functionality I created two console apps. One to show how use the generation and the other for validation. I have not written tests over these simple console applications.
![generation](http://i.imgur.com/h0i8dEG.png)

## VeOTP.Generator
Usage: 
```
VeOTP.Generator.exe shared_secret password_validity_duration_seconds password_length
```
Shows the code after `OTP: ` and presents a bar to indicate how long is remaining in its validity.
## VeOTP.Validator
Usage: 
```
VeOTP.Validator.exe shared_secret password_validity_duration_seconds password_length
```

If the same parameters are provided to the Generator and Validator, then when you type the OTP shown in the Generator it should print that it is valid. 
