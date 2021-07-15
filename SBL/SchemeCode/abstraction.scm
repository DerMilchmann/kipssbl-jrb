(define (quadrat x) (* x x))

(define summe (lambda (a b) (+ a b)))

(define comperator (lambda (x y)
                     (cond ((= x y) 0)
                           ((> x y) 1)
                           ((< x y) 1))))

(quadrat 10)
(summe 50 50)
(comperator (quadrat 10) (summe 50 50))