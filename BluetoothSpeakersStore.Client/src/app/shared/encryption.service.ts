import { Injectable } from "@angular/core";
import * as CryptoTS from 'crypto-ts';

@Injectable({
    providedIn: 'root'
})

export class EncryptionService {
    private readonly key = "Arhs!"
    constructor() {}

    encryptionAES (word:string) : string{
        // Encrypt
        const ciphertext = CryptoTS.AES.encrypt(word, this.key);
        return ciphertext.toString();
      }
    
    decryptionAES (word:string) : string{
        // Decrypt
        if (word == null) { 
            return '';
        }
        const bytes  = CryptoTS.AES.decrypt(word, this.key);
        const plaintext = bytes.toString(CryptoTS.enc.Utf8);
        return plaintext;
      }
}