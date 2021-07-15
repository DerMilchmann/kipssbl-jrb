(define (copy liste n)
  (define (copy-runner liste i)
    (if (or (null? liste) (= i n)) (list) (cons (car liste) (copy-runner (cdr liste) (+ i 1)))))
  (copy-runner liste 0))


(define (skip liste n)
  (define (runner pair i)
    (if (= i n) pair (runner (cdr pair) (+ i 1))))
  (runner liste 0))

(define (merge l r)
  (cond ((null? l) r)
        ((null? r) l)
        ((< (car l) (car r)) (cons (car l) (merge (cdr l) r)))
        ((>= (car l) (car r)) (cons (car r) (merge l (cdr r))))))


(define (merge-sort liste)
  (let ((half (round (/ (length liste) 2))))
  (if (<= (length liste) 1) liste (merge (merge-sort (copy liste half))
                                        (merge-sort (copy (skip liste half) (+ half 1)))))))
                                        
(merge-sort (list 100 90 80 20 40 50 70 30 60 10))
(merge-sort '(10 9 8 7 6 5 4 3 2 1))