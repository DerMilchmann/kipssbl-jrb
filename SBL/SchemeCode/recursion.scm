(define (summe x)(if (= x 0) 0 (+ x (summe (- x 1)))))

(define (fac n) (if (= n 0) 1 (* n (fac (- n 1)))))

(summe 100)
(fac 5)
