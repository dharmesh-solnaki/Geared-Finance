import { validationRegexes } from "../Models/constants.model";

export function generateRandomPassword() {
    const lowercase = 'abcdefghijklmnopqrstuvwxyz';
    const uppercase = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';
    const digits = '0123456789';
    const special = '!@#$%^&*';
    const allChars = lowercase + uppercase + digits + special;

    let password = '';

    // Ensure the password contains at least one character from each required set
    password += lowercase[Math.floor(Math.random() * lowercase.length)];
    password += uppercase[Math.floor(Math.random() * uppercase.length)];
    password += digits[Math.floor(Math.random() * digits.length)];
    password += special[Math.floor(Math.random() * special.length)];

    // Fill the remaining characters
    for (let i = 4; i < 8; i++) {
        password += allChars[Math.floor(Math.random() * allChars.length)];
    }

    // Shuffle the password to ensure randomness
    password = password.split('').sort(() => Math.random() - 0.5).join('');

    // Check if the password meets the regex requirements
    const regex = validationRegexes.PASSWORD_REGEX;
    if (regex.test(password)) {
        return password;
    } else {
        // If it doesn't match, generate a new one
        return generateRandomPassword();
    }
}

export function decryptPassword(encryptedString:string){
    let decrypted;
   
    try {
      const b = atob(encryptedString);
      decrypted = new TextDecoder("ascii").decode(new Uint8Array([...b].map(char => char.charCodeAt(0))));
    } catch (error) {
      decrypted = "";
    }
    return decrypted;
}