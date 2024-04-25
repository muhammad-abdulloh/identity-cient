import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IdemDetailComponent } from './idem-detail.component';

describe('IdemDetailComponent', () => {
  let component: IdemDetailComponent;
  let fixture: ComponentFixture<IdemDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [IdemDetailComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(IdemDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
