(define myappend
  (lambda (a b)
    (if (null? a) b
      (cons (car a) (myappend (cdr a) b)))))
      
(myappend (list 1 2) (list 3 4))
(myappend (quote (1 2)) '(3 4))
