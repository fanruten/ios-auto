#import "ViewController.h"


@interface ViewController ()

@end

@implementation ViewController

@synthesize textField1;
@synthesize textField2;
@synthesize resultLabel;

-(void)viewDidLoad
{
}

-(IBAction)onClickComputeSum:(id)sender
{
	int a = [[textField1 text] intValue];
	int b = [[textField2 text] intValue];
	int sum = a + b;
	NSString *newLabelValue = [NSString stringWithFormat:@"%d",sum];
	[resultLabel setText:newLabelValue];
}

@end
