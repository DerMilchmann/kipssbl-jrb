(define T (lambda (x) (lambda (y) x)))
(define F (lambda (x) (lambda (y) y)))
(define IF (lambda (p) (lambda (c) (lambda (a) ((p c) a)))))
(define I (lambda (x) x))
(define NICHT (lambda (x) ((x F) T)))
(define SUCC (lambda(n) (lambda(f) ((B f) (n f)))))

(define ZERO (lambda (f) (lambda (x) x)))
(define ONE (lambda (f) (lambda  (x) (f x))))
(define TWO (lambda (f) (lambda (x) (f (f x)))))

(define COUNTER (lambda (x) (+ x 1)))

(define TONUMBER (lambda (x) ((x COUNTER) 0)))

(define B (lambda(x) (lambda(y) (lambda(z) (x(y z))))))

(define PLUS (lambda (a) (lambda (b) (lambda (f) (lambda (x) ((a f) ((b f) x)))))))
(define PLUSSUCC (lambda (a) (lambda (b) ((a SUCC) b))))

(define MULT (lambda (a) (lambda (b) (lambda (f) (a (b f))))))

(define POW (lambda (a) (lambda (b) (b a))))

(define THREE (SUCC TWO))
(define SIX ((MULT TWO) THREE))
(define FOUR ((PLUS TWO) TWO))
(define EIGHT ((PLUSSUCC FOUR) FOUR))
(define SIXTEEN ((POW TWO) FOUR))

(define V (lambda (a) (lambda (b) (lambda (f) ((f a) b)))))

(define RCONS V)
(define RCAR (lambda (f) (f T)))
(define RCDR (lambda (f) (f F)))

((ONE COUNTER) 0)
((TWO COUNTER) 0)
((SIX COUNTER) 0)
((EIGHT COUNTER) 0)
((SIXTEEN COUNTER) 0)

(RCAR ((RCONS 10) 100))

(((((IF T) ONE) ZERO) COUNTER) 0)
(((((IF F) ZERO) ONE) COUNTER) 0) 


