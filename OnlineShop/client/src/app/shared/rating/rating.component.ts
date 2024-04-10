import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-rating',
  templateUrl: './rating.component.html',
  styleUrls: ['./rating.component.scss']
})
export class RatingComponent {
  @Output('emitOutput') emitOutput = new EventEmitter<number>();
  @Input() score! :number
  @Input() isReadonly! : boolean

  setScore(score: number) {
    this.score = score;
    this.emitOutput.emit(this.score);
  }
}
