(define (erzeuge-konto-abheben saldo)
   (lambda (betrag)
      (set! saldo (- saldo betrag))
      saldo))
      
(define konto (erzeuge-konto-abheben 1100))

(konto 0)
(konto 100)
(konto 500)
(konto (* 2 250))
