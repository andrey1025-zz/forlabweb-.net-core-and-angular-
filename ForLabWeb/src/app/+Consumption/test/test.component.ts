import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
@Component({
  selector: 'app-test',
  templateUrl: './test.component.html'
})
export class TestComponent implements OnInit {
test:string='';
testform:FormGroup;
  constructor(private _fb: FormBuilder) { }

  ngOnInit() {
    this.testform = this._fb.group({
      test1:"new",
    })
  }

}
