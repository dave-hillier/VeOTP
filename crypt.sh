KEY_FILENAME=key.txt
DURATION=30

function generate_secret_key() {
    openssl genrsa -out $KEY_FILENAME 2048
}

function generate_otp() {
    if [ ! -f $KEY_FILENAME ]; then
        echo "Generating secret key $KEY_FILENAME..."
        generate_secret_key
    fi
    echo -e "$1, $((`date +%s`+$DURATION))" | openssl rsautl -inkey $KEY_FILENAME -encrypt | base64
}

function check_valid() {
    OTP=`echo $2 | base64 -D | openssl rsautl -inkey $KEY_FILENAME -decrypt`
    USER=`echo $OTP | cut -f 1 -d,`
    EXPIRY=`echo $OTP | cut -f 2 -d,`
    NOW=`date +%s`
    if [ $NOW -lt $EXPIRY ]; then
        echo "Valid"
    else
        echo "Invalid"
    fi

    if [ $USER = $1 ]; then
        echo "Correct User"
    else
        echo "Invalid User"
    fi
}
