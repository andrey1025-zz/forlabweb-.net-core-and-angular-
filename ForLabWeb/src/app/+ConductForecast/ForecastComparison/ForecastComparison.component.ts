import { Component, OnInit, EventEmitter, Output } from '@angular/core';

import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { GlobalAPIService } from '../../shared/GlobalAPI.service';

@Component({
    selector: 'app-forecast-comparison',
    templateUrl: './ForecastComparison.component.html',
    styleUrls: ['ForecastComparison.component.css']
})

export class ForecastComparisonComponent implements OnInit {
    @Output() close = new EventEmitter()
    constructor(private _fb: FormBuilder, private _avRoute: ActivatedRoute,
        private _router: Router, private _APIwithActionService: APIwithActionService) {
    }
    ngAfterViewChecked() {
    }
    ngOnInit() {
    }


}


